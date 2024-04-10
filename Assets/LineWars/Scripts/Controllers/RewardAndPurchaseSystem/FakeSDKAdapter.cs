using System;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class FakeSDKAdapter: SDKAdapter
    {
        public override bool SDKEnabled => true;

        protected override void Initialize()
        {
            GameRoot.Instance.StartGame();
        }
        public override void LockAd()
        {
            Debug.Log("LockAd");
        }

        public override void FullscreenShow()
        {
            Debug.Log("FullScreenShow");
        }

        protected override void RewardForAd(PrizeType prizeType, int amount)
        {
            _Reward(prizeType, amount);
        }

        public override bool CanBuyProduct(string id)
        {
            return false;
        }

        public override void BuyProduct(string id)
        {
            
        }

        public override ProductData ProductByID(string id)
        {
            return null;
        }

        public override int GetProductCount()
        {
            return 0;
        }

        public override int GetProductCount(PrizeType prizeType)
        {
            return 0;
        }

        public override ProductData[] GetProducts()
        {
            return Array.Empty<ProductData>();
        }

        public override ProductData[] GetProducts(PrizeType prizeType)
        {
            return Array.Empty<ProductData>();
        }

        public override void SendMetrica(string eventName)
        {
            
        }

        public override void SendMetrica(string eventName, IDictionary<string, string> eventParams)
        {
           
        }
    }
}