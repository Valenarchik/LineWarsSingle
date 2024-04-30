using System;
using Localization;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(menuName = "Switch/MissionStatusToNameSwitch")]
    public class MissionStatusToNameSwitch: ScriptableObject
    {
        [SerializeField] private LanguageField<string> completed;
        [SerializeField] private LanguageField<string> unlocked;
        [SerializeField] private LanguageField<string> locked;
        [SerializeField] private LanguageField<string> failed;
        
        public string GetName(MissionStatus type)
        {
            return type switch
            {
                MissionStatus.Completed => completed,
                MissionStatus.Unlocked => unlocked,
                MissionStatus.Locked => locked,
                MissionStatus.Failed => failed,
                _ => unlocked
            };
        }
    }
}