using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using YG;
using YG.Utils.Pay;

namespace LineWars.Controllers
{
    public class YandexGameSdkAdapter: SDKAdapter
    {
        private static bool consume;
        private static bool dataInitialized;
        
        [Space]
        [SerializeField, Min(1)] private int priseTypeLenInBits = 10;
        [Space]
        [SerializeField] private Sprite diamondsSprite;
        [SerializeField] private string lockAdId;
        
        [Header("References")]
        [SerializeField] private YandexGameProvider yandexGameProvider;
        
        public override bool SDKEnabled => YandexGame.SDKEnabled;
        
        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent += OnErrorVideoEvent;
            YandexGame.GetPaymentsEvent += OnPaymentsEvent;
            YandexGame.PurchaseSuccessEvent += OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent += OnPurchaseFailedEvent;
            
            YandexGame.GetDataEvent += InitializeData;
            YandexGame.GetDataEvent += ConsumePurchases;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
            YandexGame.ErrorVideoEvent -= OnErrorVideoEvent;
            YandexGame.GetPaymentsEvent -= OnPaymentsEvent;
            YandexGame.PurchaseSuccessEvent -= OnPurchaseSuccessEvent;
            YandexGame.PurchaseFailedEvent -= OnPurchaseFailedEvent;
            
            YandexGame.GetDataEvent -= InitializeData;
            YandexGame.GetDataEvent -= ConsumePurchases;
        }

        protected override void Initialize()
        {
            if (SDKEnabled)
            {
                ConsumePurchases();
                InitializeData();
            }
        }
        
        private void ConsumePurchases()
        {
            if (consume) return;
            consume = true;
            
            YandexGame.ConsumePurchases();
        }
        
        private void InitializeData()
        {
            if (dataInitialized) return;
            dataInitialized = true;
            
            yandexGameProvider.LoadAll();
            GameRoot.Instance.StartGame();
            
            UsePromoCode(YandexGame.EnvironmentData.payload);
        }

        public override void FullscreenShow()
        {
            YandexGame.FullscreenShow();
        }

        protected override void RewardForAd(PrizeType prizeType, int amount)
        {
            if (!CheckEnableSdk()) return;
            
            try
            {
                var id = RewardUtilities.EncodeId(prizeType, amount, priseTypeLenInBits);
                YandexGame.RewVideoShow(id);
            }
            catch
            {
                Debug.LogError("EncodeId exception!");
                InvokeErrorVideoEvent();
            }
        }

        public override ProductData ProductByID(string id)
        {
            if (!CheckEnableSdk()) return null;
            
            return ConvertPurchase(YandexGame.PurchaseByID(id));
        }

        public override ProductData[] GetProducts() 
        {
            if (!CheckEnableSdk()) return null;
            
            return YandexGame.purchases
                .Where(CanConvertPurchase)
                .Select(ConvertPurchase)
                .ToArray();
        }

        public override ProductData[] GetProducts(PrizeType prizeType)
        {
            if (!CheckEnableSdk()) return null;
            
            return GetProducts()
                .Where(x => x.Prize != null && x.Prize.Type == prizeType)
                .ToArray();
        }

        public override void LockAd()
        {
            BuyProduct(lockAdId);
        }

        private void _LockAd()
        {
            //YandexGame.Instance.infoYG.AdWhenLoadingScene = false;
            //TODO
        }

        public override int GetProductCount()
        {
            if (!CheckEnableSdk()) return -1;
            return YandexGame.purchases.Length;
        }

        public override int GetProductCount(PrizeType prizeType)
        {
            if (!CheckEnableSdk()) return -1;
            return GetProducts(prizeType).Length;
        }
        
        public override bool CanBuyProduct(string id)
        {
            if (!CheckEnableSdk()) return false;
            return RewardUtilities.CanDecodePurchaseId(id);
        }

        public override void BuyProduct(string id)
        {
            if (!CheckEnableSdk()) return;
            
            if (!CanBuyProduct(id))
            {
                Debug.LogError($"Cant buy this Purchase {nameof(id)}={id}");
                return;
            }
            YandexGame.BuyPayments(id);
        }

        private bool CanConvertPurchase(Purchase yg)
        {
            return yg != null && RewardUtilities.CanDecodePurchaseId(yg.id);
        }
        
        private ProductData ConvertPurchase(Purchase yg)
        {
            return new ProductData(
                yg.id,
                yg.title,
                yg.description,
                diamondsSprite, 
                int.TryParse(yg.priceValue, out var value) ? value : -1, 
                "Ян",
                RewardUtilities.DecodePurchaseId(yg.id));
        }

        private void OnRewardVideoEvent(int id)
        {
            var (type, amount) = RewardUtilities.DecodeId(id, priseTypeLenInBits);
            _Reward(type, amount);
            SaveCloud();
            
            UIPanel.OpenSuccessPanel(new Money(CostType.Gold, amount));
            InvokeRewardVideoEvent(type, amount);
        }
        
        private void OnErrorVideoEvent()
        {
            UIPanel.OpenErrorPanel();
            InvokeErrorVideoEvent();
        }
        
        private void OnPurchaseSuccessEvent(string id)
        {
            if (id == lockAdId)
            {
                UserInfoController.LockAd = true;
                UIPanel.OpenSuccessLockAdPanel();
                InvokeSuccessLockAd();
                InvokePurchaseSuccessEvent(id);
                return;
            }
            
            var prize = RewardUtilities.DecodePurchaseId(id);
            _Reward(prize);
            SaveCloud();
            UIPanel.OpenSuccessPanel(new Money(prize.Type.ToCostType(), prize.Amount));
            InvokePurchaseSuccessEvent(id);
        }
        
        private void OnPurchaseFailedEvent(string id)
        {
            UIPanel.OpenErrorPanel();
            InvokePurchaseFailedEvent(id);
        }
        
        private void OnPaymentsEvent()
        {
            InvokePurchasesUpdated();
        }
        


        public override void SendMetrica(string eventName)
        {
            YandexMetrica.Send(eventName);
        }

        public override void SendMetrica(
            string eventName,
            IDictionary<string, string> eventParams)
        {
            YandexMetrica.Send(eventName, eventParams);
        }


        private void SaveCloud()
        {
#if UNITY_EDITOR
#else
            YandexGame.SaveCloud();
#endif
        }
    }
}