using System;
using LineWars.Controllers;
using LineWars.Model;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class GoldBuyPresetLogic : MonoBehaviour
    {
        [SerializeField] private BuyPanel buyPanel;
        [SerializeField] private int goldAmount;
        [SerializeField] private int costInDiamonds;

        [SerializeField] private Button buyButton;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private TMP_Text amountText;

        private static UserInfoController UserController => GameRoot.Instance.UserController;

        private void OnEnable()
        {
            ReDraw();
            buyButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            buyButton.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            buyPanel.OpenWindow(GetBuyPanelReDrawInfo());
            buyPanel.OnClick.AddListener(BuyGold);
        }

        private BuyPanelReDrawInfo GetBuyPanelReDrawInfo()
        {
            return new BuyPanelReDrawInfo
            (
                DrawHelper.GoldSprite,
                ParseAmount(goldAmount),
                GetDescription(),
                costInDiamonds,
                CostType.Diamond,
                UserController.UserDiamond >= costInDiamonds
            );
        }

        private void BuyGold()
        {
            if (UserController.UserDiamond < costInDiamonds)
                throw new InvalidOperationException();
            
            UserController.UserDiamond -= costInDiamonds;
            UserController.UserGold += goldAmount;
            
            buyPanel.OnClick.RemoveListener(BuyGold);
        }
        
        private void ReDraw()
        {
            costText.text = costInDiamonds.ToString();
            amountText.text = ParseAmount(goldAmount);
        }

        private string GetDescription()
        {
            return LocalizationManager.LangName switch
            {
                LangName.ru => $"Вы приобретаете {ParseAmount(goldAmount)} за <font=\"New Rocker\">{costInDiamonds}</font> кристаллов",
                LangName.en => $"You purchase {ParseAmount(goldAmount)} for <font=\"New Rocker\">{costInDiamonds}</font> crystals",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private string ParseAmount(int amount)
        {
            return LocalizationManager.LangName switch
            {
                LangName.ru => $"<font=\"New Rocker\">{amount}</font> монет",
                LangName.en => $"<font=\"New Rocker\">{amount}</font> coins",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}   