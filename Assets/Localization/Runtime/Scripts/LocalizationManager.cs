using System;
using UnityEngine;

namespace Localization
{
    [DefaultExecutionOrder(-50)]
    public class LocalizationManager : MonoBehaviour
    {
        private static LocalizationManager Instance;
        public static string lang = "ru";
        public static string Lang
        {
            get => lang;
            set
            {
                lang = value ?? throw new ArgumentNullException();
                SwitchLangEvent?.Invoke();
            }
        }

        public static LangName LangName
        {
            get => Enum.Parse<LangName>(lang);
            set => SetLanguage(value);
        }

        public LocalizationSettings settings;
        public static LocalizationSettings Settings => Instance?.settings;

        public static event Action SwitchLangEvent;
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private static void SetLanguage(LangName langName)
        {
            Lang = LangMethods.LangName(langName);
        }

        private void OnValidate()
        {
            Instance = this;
        }
    }
}
