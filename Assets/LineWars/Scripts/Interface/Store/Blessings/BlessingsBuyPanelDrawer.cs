using System;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class BlessingsBuyPanelDrawer : MonoBehaviour
    {
        [SerializeField] private BlessingBuyDrawer blessingBuyDrawerPrefab;
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private LayoutGroup blessingsLayout;

        private BlessingId selectedBlessing;
        private readonly List<GameObject> instances = new ();
        
        private static Store Store => GameRoot.Instance.Store;

        private void OnEnable()
        {
            buyPanel.OnClick.AddListener(BuyBlessing);
            ReDrawBoxes();
        }

        private void OnDisable()
        {
            if (buyPanel != null)
                buyPanel.OnClick.RemoveListener(BuyBlessing);
            ReleaseInstances();
        }


        private void ReDrawBoxes()
        {
            var blessings = GameRoot.Instance.Store.BlessingsForPurchase;
            foreach (var blessing in blessings)
            {
                var instance = Instantiate(blessingBuyDrawerPrefab, blessingsLayout.transform);
                instance.ReDraw(blessing);
                instance.OnClick.AddListener(() =>
                {
                    buyPanel.OpenWindow(GetBuyPanelReDrawInfo(blessing));
                    selectedBlessing = blessing;
                });
                instances.Add(instance.gameObject);
            }
        }
        
        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo(BlessingId blessingId)
        {
            return new BuyPanelReDrawInfo(
                DrawHelper.GetSpriteByBlessingID(blessingId),
                DrawHelper.GetBlessingNameByBlessingID(blessingId),
                DrawHelper.GetBlessingDescription(blessingId),
                GameRoot.Instance.Store.BlessingToCost[blessingId],
                CostType.Gold,
                Store.CanBuy(blessingId));
        }
        
        private void BuyBlessing()
        {
            if (!Store.CanBuy(selectedBlessing))
                throw new InvalidOperationException("Can't buy this Blessing");
            Store.Buy(selectedBlessing);
        }
        
        private void ReleaseInstances()
        {
            foreach (var o in instances)
            {
                if (o != null)
                    Destroy(o);
            }
            instances.Clear();
        }
    }
}
