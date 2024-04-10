using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BuyPurchaseButton: MonoBehaviour
    {
        private static SDKAdapter SDKAdapter => GameRoot.Instance.SdkAdapter;
        
        [SerializeField] private Button button;
        [SerializeField] private PurchaseContainer purchaseContainer;
        
        private void OnEnable()
        {
            button.onClick.AddListener(BuyPurchase);
            if (purchaseContainer.Data != null)
                button.interactable = SDKAdapter.CanBuyProduct(purchaseContainer.Data.Id);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(BuyPurchase);
        }
        
        private void BuyPurchase()
        {
            if (purchaseContainer.Data != null)
                SDKAdapter.BuyProduct(purchaseContainer.Data.Id);
        }
    }
}