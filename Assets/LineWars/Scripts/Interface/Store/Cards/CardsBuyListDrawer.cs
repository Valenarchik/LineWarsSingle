using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class CardsBuyListDrawer : MonoBehaviour
    {
        [SerializeField] private CardBuyElementDrawer cardDrawInfoPrefab;
        [SerializeField] private CardBuyPanel buyPanel;
        [SerializeField] private LayoutGroup boxesLayout;
        [SerializeField] private BoxImagesShower boxImagesShower;

        private DeckCard selectedCard;
        private readonly List<GameObject> instances = new ();
        
        private Store Store => GameRoot.Instance.Store;
        private IStorage<int, DeckCard> DeckCardStorage => GameRoot.Instance.CardsDatabase;

        private void OnEnable()
        {
            buyPanel.OnClick.AddListener(BuySelectedCard);
            ReDrawBoxes();
        }

        private void OnDisable()
        {
            buyPanel.OnClick.RemoveListener(BuySelectedCard);
            ReleaseInstances();
        }

        private void ReDrawBoxes()
        {
            var cards = GameRoot.Instance.Store.CardsForPurchase.Select(x => GameRoot.Instance.CardsDatabase.IdToValue[x]);
            foreach (var deckCard in cards)
            {
                var instance = Instantiate(cardDrawInfoPrefab, boxesLayout.transform);
                instance.Redraw(deckCard);
                instance.OnClickAction.AddListener(
                    () =>
                    {
                        buyPanel.OpenWindow(deckCard, Store.CanBuy(deckCard));
                        selectedCard = deckCard;
                    });
                instances.Add(instance.gameObject);
            }
        }
        
        private void BuySelectedCard()
        {
            if (selectedCard == null)
                throw new InvalidOperationException("SelectedCard in null");
            var cardId = DeckCardStorage.ValueToId[selectedCard];
            if (!Store.CanBuy(selectedCard))
                throw new InvalidOperationException("Can't buy this Card");
            Store.Buy(selectedCard);

            var contextedDropArray = new List<ContextedDrop>
            {
                new (new Drop(LootType.Card, cardId))
            };
            
            boxImagesShower.gameObject.SetActive(true);
            boxImagesShower.ShowItems(contextedDropArray);
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
