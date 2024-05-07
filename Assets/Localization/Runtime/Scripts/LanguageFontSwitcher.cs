using System;
using TMPro;
using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LanguageFontSwitcher: MonoBehaviour
    {
        [SerializeField] private bool useSettings = true;
        [SerializeField, ConditionallyVisible("useSettings")] private int fontNumber;
        [SerializeField, ConditionallyVisible("useSettings", true), LanguageField] private LanguageField<TMP_FontAsset> fonts;

        private LocalizationSettings settings;
        private TMP_Text textMPComponent;
        private float baseFontSize;
        
        private void Awake()
        {
            textMPComponent = GetComponent<TMP_Text>();
            settings = SettingsLoader.LoadSettings();
            baseFontSize = textMPComponent.fontSize;
        }

        private void OnEnable()
        {
            OnLanguageSwitch();
            LocalizationManager.SwitchLangEvent += OnLanguageSwitch;
        }
        
        private void OnDisable()
        {
            LocalizationManager.SwitchLangEvent -= OnLanguageSwitch;
        }

        private void OnLanguageSwitch()
        {
            if (useSettings)
            {
                textMPComponent.font = LangMethods.GetCurrentFont(LocalizationManager.LangName, fontNumber, settings);
                var correct = LangMethods.GetCurrentFontSizeCorrect(LocalizationManager.LangName, fontNumber, settings);
                textMPComponent.fontSize = baseFontSize + correct;
            }
            else
            {
                textMPComponent.font = fonts;
            }
        }
    }
}