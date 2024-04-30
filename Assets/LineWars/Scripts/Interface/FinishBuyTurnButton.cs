using System;
using LineWars.Controllers;
using LineWars.Model;
using Localization;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    [RequireComponent(typeof(Button))]
    public class FinishBuyTurnButton: MonoBehaviour
    {
        private const string toggleKey = "skipFinishBuyTurnDialog";
        private static UserInfoController UserController => GameRoot.Instance?.UserController;
        private static Player Player => Player.LocalPlayer;
        private static SingleGameRoot SingleGameRoot => SingleGameRoot.Instance; 
        
        private Button button;
        [SerializeField] private GameObject buyUnitsLayerContainer;
        [SerializeField] private bool openDialogPanel = true;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            if (SingleGameRoot.GameReferee is WallToWallGameReferee 
                && Player.CanBuyAnyCard())
            {
                Player.PlayerBuyDeckCard += OnPlayerBuyDeckCardWTW;
                button.interactable = false;
            }
        }

        private void OnPlayerBuyDeckCardWTW(BasePlayer basePlayer, DeckCard deckCard)
        {
            Player.PlayerBuyDeckCard -= OnPlayerBuyDeckCardWTW;
            button.interactable = true;
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }
        
        private void OnClick()
        {
            if (!openDialogPanel || UserController == null || !Player.CanBuyAnyCard())
            {
                ClosePanelAndFinishTurn();
            }
            else
            {
                var toggleIsOn = UserController.KeyToBool[toggleKey];
                if (toggleIsOn)
                {
                    ClosePanelAndFinishTurn();
                }
                else
                {
                    OpenDialogPanel(false);
                }
            }
        }
        
        private static string OpenDialogPanelTitle => LocalizationManager.LangName switch
        {
            LangName.ru => "Вы уверены что хотите завершить ход?",
            LangName.en => "Are you sure you want to complete the move?",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        private static string OpenDialogPanelDescription => LocalizationManager.LangName switch
        {
            LangName.ru => "У вас еще остались монеты, для покупки какого-то юнита.",
            LangName.en => "You still have coins left to buy some kind of unit.",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        private static string OpenDialogPanelAcceptText => LocalizationManager.LangName switch
        {
            LangName.ru => "да",
            LangName.en => "yes",
            _ => throw new ArgumentOutOfRangeException()
        };
        
        private static string OpenDialogPanelRejectText=> LocalizationManager.LangName switch
        {
            LangName.ru => "нет",
            LangName.en => "no",
            _ => throw new ArgumentOutOfRangeException()
        };
        private static string OpenDialogPanelToggleText => LocalizationManager.LangName switch
        {
            LangName.ru => "Не показывать это окно больше",
            LangName.en => "Don't show this window anymore",
            _ => throw new ArgumentOutOfRangeException()
        };
        private void OpenDialogPanel(bool toggleIsOn)
        {
            UIPanel.OpenDialogPanel(
                OpenDialogPanelTitle,
                OpenDialogPanelDescription,
                OpenDialogPanelAcceptText,
                OpenDialogPanelRejectText,
                OpenDialogPanelToggleText,
                toggleIsOn,
                DialogOnAccept,
                DialogOnReject
                );
        }

        private void DialogOnAccept(bool isOn)
        {
            UserController.KeyToBool[toggleKey] = isOn;
            ClosePanelAndFinishTurn();
        }

        private void DialogOnReject(bool isOn)
        {
            UserController.KeyToBool[toggleKey] = isOn;
        }
        
        private void ClosePanelAndFinishTurn()
        {
            buyUnitsLayerContainer.SetActive(false);
            Player.FinishTurn();
        }
    }
}