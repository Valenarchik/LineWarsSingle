using Localization;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class YGLanguageSwitcher : MonoBehaviour
{
    public UnityEvent onSwitchLanguage; 
    private void Start()
    {
        LocalizationManager.Lang = YandexGame.lang;
    }

    private void OnEnable()
    {
        YandexGame.SwitchLangEvent += OnSwitchLangEvent;
    }

    private void OnDisable()
    {
        YandexGame.SwitchLangEvent -= OnSwitchLangEvent;
    }

    private void OnSwitchLangEvent(string lang)
    {
        LocalizationManager.Lang = lang;
        onSwitchLanguage.Invoke();
    }

    [EditorButton]
    public void RU()
    {
        LocalizationManager.LangName = LangName.ru;
    }

    [EditorButton]
    private void EN()
    {
        LocalizationManager.LangName = LangName.en;
    }
    
    [EditorButton]
    private void TR()
    {
        LocalizationManager.LangName = LangName.tr;
    }
}
