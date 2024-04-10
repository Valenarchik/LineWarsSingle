using UnityEngine;
using LineWars.Model;
using RuStore.BillingClient;
using RuStore;
using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using UnityEngine.SceneManagement;
using YandexMobileAds.Base;
using LineWars.Controllers;

namespace RuStoreLineWars
{
    public class RuStoreAdapter : SDKAdapter
    {
        [SerializeField] private string[] productsId;
        [SerializeField] private Sprite diamondsSprite;
        [SerializeField] private float adFreePause;
        [SerializeField] private string interstitialAdUnitId = "demo-interstitial-yandex";
        [SerializeField] private string rewardAdUnitId = "demo-rewarded-yandex";
        
        private YandexInterstitialAd interstitialAd;
        private YandexRewardAd rewardAd;
        private RuStoreBillingClient ruStoreBillingClient;
        private bool isPurchasesAvailable;
        private List<ProductData> products;
        private float timeSinceLastAd;
        
        public override bool SDKEnabled => isPurchasesAvailable;

        protected override void Initialize()
        {
            interstitialAd = new YandexInterstitialAd(interstitialAdUnitId);
            rewardAd = new YandexRewardAd(rewardAdUnitId);
            
            ruStoreBillingClient = RuStoreBillingClient.Instance;
            ruStoreBillingClient.Init();
            
            CheckPurchasesAvailability();
            timeSinceLastAd = adFreePause;
            GameRoot.Instance.StartGame();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneChanged;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
        }

        private void Update()
        {
            timeSinceLastAd += Time.unscaledDeltaTime;
        }

        private void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            if (timeSinceLastAd >= adFreePause)
            {
                timeSinceLastAd = 0;
                FullscreenShow();
            }
        }

        private void CheckPurchasesAvailability()
        {
            ruStoreBillingClient.CheckPurchasesAvailability(OnError, OnSuccess);
            
            void OnSuccess(FeatureAvailabilityResult featureAvailabilityResult)
            {
                isPurchasesAvailable = featureAvailabilityResult.isAvailable;
                Debug.Log("CPA Success: " +  isPurchasesAvailable);
                InitPurchases();
            }

            void OnError(RuStoreError error)
            {
                isPurchasesAvailable = false;
                Debug.Log("CPA error");
                Debug.Log(error.name);
                Debug.Log(error.description);
            }
        }

        private void FinishPurchases()
        {
            ruStoreBillingClient.GetPurchases(OnFailure, OnSuccess);

            void OnFailure(RuStoreError error)
            {
                Debug.Log("Loading purchases failed");
            }

            void OnSuccess(List<Purchase> purchases)
            {
                foreach(var currentPurchase in purchases)
                {
                    if(currentPurchase.productType == Product.ProductType.CONSUMABLE &&
                        currentPurchase.purchaseState == Purchase.PurchaseState.PAID)
                        ConfirmSuccessfulPurchase(currentPurchase.purchaseId, currentPurchase.productId);
                }
            }
        }

        private void InitPurchases()
        {
            ruStoreBillingClient.GetProducts(productsId, OnError, OnSuccess);

            void OnError(RuStoreError error)
            {
                Debug.Log("Init Purchases error: " + error.name);
                products = new List<ProductData>();
            }

            void OnSuccess(List<Product> product)
            {
                products = new();
                Debug.Log("Init purchases success");
                foreach (var item in product)
                {
                    Debug.Log(item.currency);
                    Debug.Log(item.price);
                    products.Add(new ProductData(
                    item.productId,
                    item.title,
                    item.description,
                    diamondsSprite,
                    item.price / 100,
                    item.currency,
                    RewardUtilities.DecodePurchaseId(item.productId)));
                }

                products.Sort((p1, p2) => p1.PriceValue.CompareTo(p2.PriceValue));
                FinishPurchases();
            }
        }

        public override void BuyProduct(string id)
        {
            if (!CheckEnableSdk())
                return;

            ruStoreBillingClient.PurchaseProduct(id, 1, null, OnError, OnSuccess);

            void OnError(RuStoreError error)
            {
                OnPurchaseFailedEvent(id);
            }

            void OnSuccess(PaymentResult paymentResult)
            {
                ConfirmPurchase(paymentResult);
            }
        }

        private void OnPurchaseFailedEvent(string id)
        {
            UIPanel.OpenErrorPanel();
            InvokePurchaseFailedEvent(id);
        }

        private void ConfirmPurchase(PaymentResult paymentResult)
        {
            if(paymentResult is PaymentCancelled cancelledPayment)
            {
                OnPurchaseFailedEvent(cancelledPayment.purchaseId);
                return;
            }
            if(paymentResult is PaymentFailure failedPayment)
            {
                OnPurchaseFailedEvent(failedPayment.purchaseId);
                return;
            }
            if(paymentResult is PaymentSuccess successPayment)
            {
                ConfirmSuccessfulPurchase(successPayment.purchaseId, successPayment.productId);
            }
        }

        private void ConfirmSuccessfulPurchase(string purchaseId, string productId)
        {
            ruStoreBillingClient.ConfirmPurchase(purchaseId, OnError, OnSuccess);

            void OnError(RuStoreError error)
            {
                OnPurchaseFailedEvent(purchaseId);
            }

            void OnSuccess()
            {
                var prize = RewardUtilities.DecodePurchaseId(productId);
                _Reward(prize);
                UIPanel.OpenSuccessPanel(new Money(prize.Type.ToCostType(), prize.Amount));
                InvokePurchaseSuccessEvent(purchaseId);
            }
        }

        public override bool CanBuyProduct(string id)
        {
            if (!CheckEnableSdk()) return false;
            return RewardUtilities.CanDecodePurchaseId(id);
        }

        public override void FullscreenShow()
        {
            interstitialAd.ShowInterstitial();
        }

        public override int GetProductCount()
        {
            if (!CheckEnableSdk())
                return -1;

            return products.Count;
        }

        public override int GetProductCount(PrizeType prizeType)
        {
            if (!CheckEnableSdk())
                return -1;

            return products.Count(
                x => x.Prize != null 
                     && x.Prize.Type == prizeType);
        }

        public override ProductData[] GetProducts()
        {
            if (!CheckEnableSdk())
                return null;

            return products.ToArray();
        }

        public override ProductData[] GetProducts(PrizeType prizeType)
        {
            return products
                .Where(x => x.Prize != null && x.Prize.Type == prizeType)
                .ToArray();
        }
        
        public override ProductData ProductByID(string id)
        {
            return products.FirstOrDefault(item => item.Id == id);
        }
        
        protected override void RewardForAd(PrizeType prizeType, int amount)
        { 
            rewardAd.Rewarded += OnRewarded;
            rewardAd.ShowRewardedAd();

            void OnRewarded(object _, Reward _1)
            {
                rewardAd.Rewarded -= OnRewarded;
                _Reward(prizeType, amount);

                UIPanel.OpenSuccessPanel(new Money(CostType.Gold, amount));
                InvokeRewardVideoEvent(prizeType, amount);
            }
        }
        
        public override void LockAd()
        {
        }
        
        public override void SendMetrica(string eventName)
        {
        }

        public override void SendMetrica(string eventName, IDictionary<string, string> eventParams)
        {
        }
    }
}
