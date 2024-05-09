using System;
using Localization;
using UnityEngine;

namespace LineWars
{
    [Serializable]
    public class GameRefereeInfo
    {
        [SerializeField] private LanguageField<string> name;
        [SerializeField] private LanguageField<string> description;

        public string Name => name;
        public string Description => description;
    }
}