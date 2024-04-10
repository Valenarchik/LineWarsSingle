using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Infrastructure
{
    [RequireComponent(typeof(BasePlayer))]
    public class MetricaPlayerListener: MonoBehaviour
    {
        private BasePlayer basePlayer;
        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;

        private void Awake()
        {
            basePlayer = GetComponent<BasePlayer>();
        }

        private void OnEnable()
        {
            basePlayer.PlayerBuyDeckCard += OnBuyCard;
        }
        
        private void OnDisable()
        {
            basePlayer.PlayerBuyDeckCard -= OnBuyCard;
        }
        
        private void OnBuyCard(BasePlayer player, DeckCard deckCard)
        {
            if (SDKAdapter != null)
                SDKAdapter.SendUseDeckCardMetrica(deckCard);
        }
    }
}