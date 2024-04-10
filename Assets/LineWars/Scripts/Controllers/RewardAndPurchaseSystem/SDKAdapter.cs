using System;
using System.Collections.Generic;
using LineWars.Infrastructure;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Controllers
{
    public abstract class SDKAdapter: MonoBehaviour
    {
        [SerializeField] private UnityEvent<PrizeType, int> rewardVideoEvent;
        [SerializeField] private UnityEvent errorVideoEvent;
        [SerializeField] private UnityEvent purchasesUpdatedEvent;
        [SerializeField] private UnityEvent<string> purchaseSuccessEvent;
        [SerializeField] private UnityEvent<string> purchaseFailedEvent;
        [SerializeField] private UnityEvent successLockAd;
        
        public event Action<PrizeType, int> RewardVideoEvent;
        public event Action ErrorVideoEvent;
        public event Action PurchasesUpdated;
        public event Action<string> PurchaseSuccessEvent;
        public event Action<string> PurchaseFailedEvent;
        public event Action SuccesLockAd; 
        
        protected UserInfoController UserInfoController => GameRoot.Instance.UserController;
        private IDictionary<string, Prize> _promocodes;

        public void Initialize(IDictionary<string, Prize> promocodes)
        {
            _promocodes = promocodes;
            Initialize();
        }

        protected abstract void Initialize();
        public abstract bool SDKEnabled { get; }
        public abstract void LockAd();
        public abstract void FullscreenShow();
        protected abstract void RewardForAd(PrizeType prizeType, int amount);
        public abstract bool CanBuyProduct(string id);
        public abstract void BuyProduct(string id);
        public abstract ProductData ProductByID(string id);
        public abstract int GetProductCount();
        public abstract int GetProductCount(PrizeType prizeType);
        public abstract ProductData[] GetProducts();
        public abstract ProductData[] GetProducts(PrizeType prizeType);
      
        
        public void RewardForAd(Prize prize) => RewardForAd(prize.Type, prize.Amount); 
        protected void _Reward(Prize prize)
        {
            _Reward(prize.Type, prize.Amount);
        }
        protected void _Reward(PrizeType prizeType, int amount)
        {
            switch (prizeType)
            {
                case PrizeType.Gold:
                    UserInfoController.UserGold += amount;
                    break;
                case PrizeType.Diamonds:
                    UserInfoController.UserDiamond += amount;
                    break;
                case PrizeType.UpgradeCards:
                    UserInfoController.UserUpgradeCards += amount;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(prizeType), prizeType, null);
            }
        }
        public void Reward(Prize prize)
        {
            _Reward(prize);
            UIPanel.OpenSuccessPanel(prize);
        }
        protected void InvokeRewardVideoEvent(PrizeType prizeType, int amount)
        {
            rewardVideoEvent?.Invoke(prizeType, amount);
            RewardVideoEvent?.Invoke(prizeType, amount);
        }
        
        protected void InvokeErrorVideoEvent()
        {
            errorVideoEvent?.Invoke();
            ErrorVideoEvent?.Invoke();
        }

        protected void InvokePurchasesUpdated()
        {
            purchasesUpdatedEvent?.Invoke();
            PurchasesUpdated?.Invoke();
        }
        
        protected void InvokePurchaseSuccessEvent(string id)
        {
            purchaseSuccessEvent?.Invoke(id);
            PurchaseSuccessEvent?.Invoke(id);
        }
        
        protected void InvokePurchaseFailedEvent(string id)
        {
            purchaseFailedEvent?.Invoke(id);
            PurchaseFailedEvent?.Invoke(id);
        }

        protected void InvokeSuccessLockAd()
        {
            successLockAd?.Invoke();
            SuccesLockAd?.Invoke();
        }

        public abstract void SendMetrica(string eventName);
        public abstract void SendMetrica(string eventName, IDictionary<string, string> eventParams);
        
        protected bool CheckEnableSdk()
        {
            if (!SDKEnabled)
                DebugUtility.LogError("SDK is not enabled!");
            return SDKEnabled;
        }
        
        public void SendDeckMetrica(Deck deck)
        {
            if (deck == null)
                return;
            
            foreach (var card in deck.Cards)
                SendDeckCardMetrica(card);
        }

        public void SendDeckCardMetrica(DeckCard deckCard)
        {
            if (deckCard == null)
                return;
            
            var eventParams = new Dictionary<string, string>
            {
                {"cardType", deckCard.Unit.Type.ToString()}
            };
            SendMetrica("card", eventParams);
        }

        public void SendUseDeckCardMetrica(DeckCard deckCard)
        {
            if (deckCard == null)
                return;
            var eventParams = new Dictionary<string, string>
            {
                {"UseCardType", deckCard.Unit.Type.ToString()}
            };
            SendMetrica("useCard", eventParams);
        }

        public void SendButtonMetrica(string buttonId)
        {
            if (buttonId == null)
                return;
            
            var eventParams = new Dictionary<string, string>
            {
                {"buttonId", buttonId},
            };
            
            SendMetrica("button", eventParams);
        }

        public void SendStartLevelMetrica(SceneName scene)
        {
            var eventParams = new Dictionary<string, string>
            {
                {"startLevel", scene.ToString()},
            };
            
            SendMetrica("missionStart", eventParams);
        }

        public void SendFinishLevelMetrica(SceneName scene, LevelFinishStatus finishStatus, int round)
        {
            var eventParams = new Dictionary<string, string>
            {
                {"endLevel", $"{scene} {finishStatus}"},
                {"round", round.ToString()}
            };
            SendMetrica("missionEnd", eventParams);
        }
        
        public void SendFinishLevelMetrica(SceneName scene, LevelFinishStatus finishStatus)
        {
            var eventParams = new Dictionary<string, string>
            {
                {"endLevel", $"{scene} {finishStatus}"}
            };
            SendMetrica("missionEnd", eventParams);
        }
        
        public void SendStartEndlessLevelMetrica(SceneName scene, InfinityGameMode infinityGameMode)
        {
            var eventParams = new Dictionary<string, string>
            {
                {"startLevel", $"{scene} {infinityGameMode}"},
            };
            
            SendMetrica("missionStart", eventParams);
        }
        
        public void SendFinishEndlessLevelMetrica(
            SceneName scene, 
            InfinityGameMode infinityGameMode,
            LevelFinishStatus finishStatus)
        {
            var eventParams = new Dictionary<string, string>
            {
                {"endLevel", $"{scene} {infinityGameMode} {finishStatus}"},
            };
            SendMetrica("missionEnd", eventParams);
        }

        public void SendFirst60SecondsInGameMetrica()
        {
            SendMetrica("first60sec");
        }

        public void SendFirstEducationMetrica()
        {
            SendMetrica("firstEducation");
        }

        public void SendFirstCompanyMetrica()
        {
            SendMetrica("firstCompany");
        }

        public void SendFirstPurchaseMetrica()
        {
            SendMetrica("firstPurchase");
        }

        public void SendFirstButtonMetrica(string buttonId)
        {
            var eventParams = new Dictionary<string, string>
            {
                {"firstButtonId", buttonId},
            };
            SendMetrica("firstButton", eventParams);
        }

        public void SendBlessingMetrica(BlessingId blessingId)
        {
            if (blessingId == BlessingId.Null)
                return;
            
            var eventParams = new Dictionary<string, string>
            {
                {"blessing", blessingId.ToString()},
            };
            SendMetrica("blessing", eventParams);
        }

        public void SendUseBlessingMetrica(BlessingId blessingId)
        {
            if (blessingId == BlessingId.Null)
                return;
            var eventParams = new Dictionary<string, string>
            {
                {"useBlessing", blessingId.ToString()},
            };
            SendMetrica("useBlessing", eventParams);
        }

        public void SendBlessingsMetrica(IBlessingsPull blessingsPull)
        {
            if (blessingsPull == null)
                return;
            
            foreach (var (blessingId, count) in blessingsPull)
            {
                if (count != 0)
                    SendBlessingMetrica(blessingId);
            }
        }

        public void SendBuyBlessingMetrica(BaseBlessing baseBlessing)
        {
            if (baseBlessing == null || baseBlessing.BlessingId == BlessingId.Null)
                return;
            
            var eventParams = new Dictionary<string, string>
            {
                {"buyBlessing", baseBlessing.BlessingId.ToString()},
            };
            
            SendMetrica("store", eventParams);
        }
        
        public void SendBuyCardMetrica(DeckCard deckCard)
        {
            if (deckCard == null)
                return;

            var eventParams = new Dictionary<string, string>
            {
                {"buyCard", deckCard.Unit.Type.ToString()}
            };
            
            SendMetrica("store", eventParams);
        }

        public void SendOpenCaseMetrica(LootBoxType lootBoxType)
        {
            var eventParams = new Dictionary<string, string>
            {
                {"openCase", lootBoxType.ToString()}
            };
            
            SendMetrica("store", eventParams);
        }

        public void SendUsePromocodeMetrica(string promocode)
        {
            if (string.IsNullOrEmpty(promocode))
                return;
            
            var eventParams = new Dictionary<string, string>
            {
                {"usePromocode", promocode}
            };
            
            SendMetrica("usePromocode", eventParams);
        }
        
        public bool UsePromoCode(string promoCode)
        {
            if (!string.IsNullOrEmpty(promoCode)
                && _promocodes.TryGetValue(promoCode, out var prize)
                && !UserInfoController.PromoCodeIsUsed(promoCode))
            {
                _Reward(prize);
                UserInfoController.UsePromoCode(promoCode);
                UIPanel.OpenSuccessUsePromoCodePanel(promoCode, prize);
                SendUsePromocodeMetrica(promoCode);
                return true;
            }
            return false;
        }
    }
}