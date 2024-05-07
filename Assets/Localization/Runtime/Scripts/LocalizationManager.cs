using System;
using UnityEngine;

namespace Localization
{
    [DefaultExecutionOrder(-50)]
    public class LocalizationManager : MonoBehaviour
    {
        private static LocalizationManager Instance;
        [SerializeField] private string lang = "ru";
        public static string Lang
        {
            get => Instance.lang;
            set
            {
                Instance.lang = value ?? throw new ArgumentNullException();
                SwitchLangEvent?.Invoke();
            }
        }

        public static LangName LangName
        {
            get => Enum.Parse<LangName>(Instance.lang);
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
            Lang = LangMethods.GetLangName(langName);
        }

        private void OnValidate()
        {
            Instance = this;
        }
    }
}
