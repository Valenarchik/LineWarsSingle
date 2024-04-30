using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class UnlmitedGameStarter : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text missionDescription;
        [SerializeField] private Image missionImage;
        
        [SerializeField] [LanguageTextArea] private LanguageField<string> langSimpleDescription;
        [SerializeField] [LanguageTextArea] private LanguageField<string> langMediumDescription;
        [SerializeField] [LanguageTextArea] private LanguageField<string> langHardDescription;
        [SerializeField] [LanguageTextArea] private LanguageField<string> langInsaneDescription;


        [SerializeField] private Color easyColor;
        [SerializeField] private Color middleColor;
        [SerializeField] private Color hardColor;
        [SerializeField] private Color insaneColor;


        private Dictionary<InfinityGameMode, (Func<string>, Color)> tuplesDict;


        private InfinityGameMode infinityGameMode;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
            missionImage.color = Color.white;

            tuplesDict = new Dictionary<InfinityGameMode, (Func<string>, Color)>
            {
                { InfinityGameMode.Simple, (() => langSimpleDescription, easyColor) },
                { InfinityGameMode.Medium, (() => langMediumDescription, middleColor) },
                { InfinityGameMode.Hard, (() => langHardDescription, hardColor) },
                { InfinityGameMode.Insane, (() => langInsaneDescription, insaneColor) },
            };
        }

        private void OnButtonClick()
        {
            InfinityGame.Load(infinityGameMode);
        }

        public void ChooseEasy() => ChooseMode(InfinityGameMode.Simple);

        public void ChooseMedium() => ChooseMode(InfinityGameMode.Medium);

        public void ChooseHard() => ChooseMode(InfinityGameMode.Hard);
        public void ChooseInsane() => ChooseMode(InfinityGameMode.Insane);

        private void ChooseMode(InfinityGameMode gameMode)
        {
            infinityGameMode = gameMode;
            var tuple = tuplesDict[gameMode];
            missionDescription.text = tuple.Item1.Invoke();
            missionImage.color = tuple.Item2;
        }
    }
}
