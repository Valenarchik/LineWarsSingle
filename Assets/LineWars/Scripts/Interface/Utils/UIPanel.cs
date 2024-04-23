using System;
using DataStructures;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace LineWars.Interface
{
    public class UIPanel : Singleton<UIPanel>
    {
        [SerializeField] private FullScreenPanel fullScreenPanel;
        [SerializeField] private DialogPanel dialogPanel;
        [SerializeField] private PromocodePanel promocodePanel;

        private IPanel[] Panels = new IPanel[0];

        private void Start()
        {
            Panels = new IPanel[] {fullScreenPanel, dialogPanel, promocodePanel};
            dialogPanel.Initialize();
            promocodePanel.Initialize();
        }
        
        public static void OpenPromoCodePanel(Func<string, bool> onEntered)
        {
            if (Instance != null)
                Instance.promocodePanel.OpenPanel(onEntered);
            else
                Debug.LogError($"{nameof(UIPanel)} is null!");
        }

        public static void OpenDialogPanel(
            TextInfo title,
            TextInfo description,
            TextInfo acceptText,
            TextInfo rejectText,
            Action onAccept,
            Action onReject)
        {
            CloseAllPanels();
            if (Instance != null)
                Instance.dialogPanel.OpenPanel(title, description, acceptText, rejectText, onAccept, onReject);
            else
                Debug.LogError($"{nameof(UIPanel)} is null!");
        }

        public static void OpenDialogPanel(
            TextInfo title,
            TextInfo description,
            TextInfo acceptText,
            TextInfo rejectText,
            TextInfo toggleText,
            bool toggleIsOn,
            Action<bool> onAccept,
            Action<bool> onReject)
        {
            CloseAllPanels();

            if (Instance != null)
                Instance.dialogPanel.OpenPanel(
                    title,
                    description,
                    acceptText,
                    rejectText,
                    toggleText,
                    toggleIsOn,
                    onAccept,
                    onReject
                );
            else
                Debug.LogError($"{nameof(UIPanel)} is null!");
        }

        public void OpenDialogPanel(
            string title,
            string description,
            string acceptText,
            string rejectText,
            Action onAccept,
            Action onReject)
        {
            CloseAllPanels();
            if (Instance != null)
                Instance.dialogPanel.OpenPanel(title,
                    description,
                    acceptText,
                    rejectText,
                    onAccept,
                    onReject);
            else
                Debug.LogError($"{nameof(UIPanel)} is null!");
        }

        public static void OpenDialogPanel(
            string title,
            string description,
            string acceptText,
            string rejectText,
            string toggleText,
            bool toggleIsOn,
            Action<bool> onAccept,
            Action<bool> onReject)
        {
            CloseAllPanels();

            if (Instance != null)
                Instance.dialogPanel.OpenPanel(
                    title,
                    description,
                    acceptText,
                    rejectText,
                    toggleText,
                    toggleIsOn,
                    onAccept,
                    onReject);
            else
                Debug.LogError($"{nameof(UIPanel)} is null!");
        }


        public static void OpenFullscreenPanel(TextInfo title, TextInfo description)
        {
            CloseAllPanels();
            if (Instance != null)
                Instance.fullScreenPanel.OpenPanel(title, description);
            else
                Debug.LogError($"{nameof(UIPanel)} is null!");
        }

        public static void OpenFullscreenPanel(TextInfo title, Money money)
        {
            CloseAllPanels();
            if (Instance != null)
                Instance.fullScreenPanel.OpenPanel(title, money);
            else
                Debug.LogError($"{nameof(UIPanel)} is null!");
        }

        public static void OpenErrorPanel()
        {
            OpenFullscreenPanel(
                new TextInfo("Ой-ой", Color.white),
                new TextInfo("произошла ошибка", Color.white)
            );
        }

        public static void OpenSuccessPanel(Money money)
        {
            OpenFullscreenPanel(new TextInfo("Успешно", Color.red), money);
        }

        public static void OpenSuccessPanel(Prize prize)
        {
            OpenFullscreenPanel(new TextInfo("Успешно", Color.red), new Money(prize.Type.ToCostType(), prize.Amount));
        }

        public static void OpenSuccessUsePromoCodePanel(string promoCode, Prize value)
        {
            OpenFullscreenPanel(
                new TextInfo("Активация промокода", Color.red),
                new TextInfo(
                    $"Вы успешно активировали промокод \"{promoCode}\" " +
                    $"Награда - {value}", 
                    Color.white
                )
            );
        }

        public static void OpenSuccessLockAdPanel()
        {
            OpenFullscreenPanel(
                new TextInfo("Отключение рекламы прошло успешно", Color.red),
                new TextInfo($"Вы успешно отключили рекламу", Color.white));
        }

        private static void CloseAllPanels()
        {
            if (Instance == null)
                return;
            foreach (var panel in Instance.Panels)
                panel.ClosePanel();
        }

        [Serializable]
        private class FullScreenPanel : IPanel
        {
            public TMP_Text title;
            public TMP_Text description;
            public MoneyDrawer moneyDrawer;
            public CanvasGroup canvasGroup;

            public void OpenPanel(TextInfo title, TextInfo description)
            {
                this.canvasGroup.alpha = 1;
                this.canvasGroup.blocksRaycasts = true;
                if (this.title)
                {
                    this.title.text = title.Text;
                    this.title.color = title.Color;
                    this.title.gameObject.SetActive(true);
                }

                if (this.description)
                {
                    this.description.text = description.Text;
                    this.description.color = description.Color;
                    this.description.gameObject.SetActive(true);
                }

                if (this.moneyDrawer)
                {
                    this.moneyDrawer.gameObject.SetActive(false);
                }
            }

            public void OpenPanel(TextInfo title, Money money)
            {
                this.canvasGroup.alpha = 1;
                this.canvasGroup.blocksRaycasts = true;
                if (this.title)
                {
                    this.title.text = title.Text;
                    this.title.color = title.Color;
                    this.title.gameObject.SetActive(true);
                }

                if (this.description)
                {
                    this.description.gameObject.SetActive(false);
                }

                if (this.moneyDrawer)
                {
                    this.moneyDrawer.Redraw(money);
                    this.moneyDrawer.gameObject.SetActive(true);
                }
            }

            public void ClosePanel()
            {
                this.canvasGroup.alpha = 0;
                this.canvasGroup.blocksRaycasts = false;
            }
        }

        [Serializable]
        private class DialogPanel : IPanel
        {
            public TMP_Text Title;
            public TMP_Text Description;
            public TMP_Text AcceptText;
            public TMP_Text RejectText;
            public TMP_Text ToggleText;

            public Toggle Toggle;
            public Button AcceptButton;
            public Button RejectButton;

            public CanvasGroup panelGroup;

            private Action acceptAction;
            private Action rejectAction;

            private Action<bool> acceptActionToggle;
            private Action<bool> rejectActionToggle;

            public void Initialize()
            {
                if (AcceptButton)
                    AcceptButton.onClick.AddListener(AcceptButtonOnClick);
                if (RejectButton)
                    RejectButton.onClick.AddListener(RejectButtonOnClick);
            }

            public void OpenPanel(
                TextInfo title,
                TextInfo description,
                TextInfo acceptText,
                TextInfo rejectText,
                Action onAccept,
                Action onReject)
            {
                ClearAllEvents();
                _OpenPanel(title, description, acceptText, rejectText);

                acceptAction += onAccept;
                rejectAction += onReject;

                if (Toggle)
                    Toggle.gameObject.SetActive(false);
            }

            public void OpenPanel(
                TextInfo title,
                TextInfo description,
                TextInfo acceptText,
                TextInfo rejectText,
                TextInfo toggleText,
                bool toggleIsOn,
                Action<bool> onAccept,
                Action<bool> onReject)
            {
                ClearAllEvents();
                _OpenPanel(title, description, acceptText, rejectText);

                if (ToggleText)
                {
                    ToggleText.text = toggleText.Text;
                    ToggleText.color = toggleText.Color;
                }

                if (Toggle)
                {
                    Toggle.gameObject.SetActive(true);
                    Toggle.isOn = toggleIsOn;
                }

                acceptActionToggle += onAccept;
                rejectActionToggle += onReject;
            }

            public void OpenPanel(
                string title,
                string description,
                string acceptText,
                string rejectText,
                Action onAccept,
                Action onReject)
            {
                OpenPanel(
                    new TextInfo(title, Color.white),
                    new TextInfo(description, Color.white),
                    new TextInfo(acceptText, Color.green),
                    new TextInfo(rejectText, Color.red),
                    onAccept,
                    onReject);
            }

            public void OpenPanel(
                string title,
                string description,
                string acceptText,
                string rejectText,
                string toggleText,
                bool toggleIsOn,
                Action<bool> onAccept,
                Action<bool> onReject)
            {
                OpenPanel(
                    new TextInfo(title, Color.white),
                    new TextInfo(description, Color.white),
                    new TextInfo(acceptText, Color.green),
                    new TextInfo(rejectText, Color.red),
                    new TextInfo(toggleText, Color.white),
                    toggleIsOn,
                    onAccept,
                    onReject);
            }

            private void AcceptButtonOnClick()
            {
                acceptAction?.Invoke();
                if (Toggle)
                    acceptActionToggle.Invoke(Toggle.isOn);

                ClearAllEvents();
                ClosePanel();
            }


            private void RejectButtonOnClick()
            {
                rejectAction?.Invoke();
                if (Toggle)
                    rejectActionToggle.Invoke(Toggle.isOn);

                ClearAllEvents();
                ClosePanel();
            }

            private void ClearAllEvents()
            {
                acceptAction = null;
                rejectAction = null;
                acceptActionToggle = null;
                rejectActionToggle = null;
            }

            public void ClosePanel()
            {
                panelGroup.blocksRaycasts = false;
                panelGroup.alpha = 0;
            }

            private void _OpenPanel(
                TextInfo title,
                TextInfo description,
                TextInfo acceptText,
                TextInfo rejectText)
            {
                _OpenPanel();
                if (Title)
                {
                    Title.text = title.Text;
                    Title.color = title.Color;
                }

                if (Description)
                {
                    Description.text = description.Text;
                    Description.color = description.Color;
                }

                if (AcceptText)
                {
                    AcceptText.text = acceptText.Text;
                    AcceptText.color = acceptText.Color;
                }

                if (RejectText)
                {
                    RejectText.text = rejectText.Text;
                    RejectText.color = rejectText.Color;
                }
            }

            private void _OpenPanel()
            {
                panelGroup.blocksRaycasts = true;
                panelGroup.alpha = 1;
            }
        }

        [Serializable]
        private class PromocodePanel: IPanel
        {
            [SerializeField] private GameObject panelObject;
            [SerializeField] private TMP_InputField inputField;
            [SerializeField] private InputField obsolete_inputField;
            [SerializeField] private int characterLimit = 9;
            [SerializeField] private GameObject onErrorPanel;

            private Func<string, bool> onEntered;
            
            public void Initialize()
            {
                if (inputField)
                {
                    inputField.text = "";
                    inputField.characterLimit = characterLimit;
                    inputField.onValueChanged.AddListener(OnValueChanged);
                }
                else if (obsolete_inputField)
                {
                    obsolete_inputField.text = "";
                    obsolete_inputField.characterLimit = characterLimit;
                    obsolete_inputField.onValueChanged.AddListener(OnValueChanged);
                }
            }

            private void OnValueChanged(string value)
            {
                onErrorPanel.gameObject.SetActive(false);
                if (value.Length < characterLimit)
                    return;
                if (onEntered == null)
                    return;

                var success= onEntered(value.ToUpper());
                
                if (!success)
                    onErrorPanel.gameObject.SetActive(true);
                else
                {
                    if (inputField)
                        inputField.text = "";
                    else if (obsolete_inputField)
                    {
                        obsolete_inputField.text = "";
                    }
                }
            }

            public void OpenPanel(Func<string, bool> onEntered)
            {
                panelObject.gameObject.SetActive(true);
                this.onEntered = onEntered;
            }

            public void ClosePanel()
            {
                panelObject.SetActive(false);
            }
        }
        
        private interface IPanel
        {
            public void ClosePanel();
        }
    }
}