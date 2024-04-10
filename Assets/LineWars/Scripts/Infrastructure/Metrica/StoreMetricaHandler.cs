using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Infrastructure
{
    public class StoreMetricaHandler: MonoBehaviour
    {
        private static Store Store => GameRoot.Instance?.Store;
        private static LootBoxController LootBoxController => GameRoot.Instance?.LootBoxController;
        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;

        private void Start()
        {
            Store.OnBuyBlessing += StoreOnBuyBlessing;
            Store.OnBuyCard += StoreOnBuyCard;
            LootBoxController.OnBoxOpen += OnBoxOpen;
        }

        private void OnDestroy()
        {
            if (Store != null)
            {
                Store.OnBuyBlessing -= StoreOnBuyBlessing;
                Store.OnBuyCard -= StoreOnBuyCard;
            }

            if (LootBoxController != null)
            {
                LootBoxController.OnBoxOpen -= OnBoxOpen;
            }
        }

        private void OnBoxOpen(LootBoxType type)
        {
            SDKAdapter.SendOpenCaseMetrica(type);
        }

        private void StoreOnBuyCard(DeckCard card)
        {
            SDKAdapter.SendBuyCardMetrica(card);
        }

        private void StoreOnBuyBlessing(BaseBlessing blessing)
        {
            SDKAdapter.SendBuyBlessingMetrica(blessing);
        }
    }
}