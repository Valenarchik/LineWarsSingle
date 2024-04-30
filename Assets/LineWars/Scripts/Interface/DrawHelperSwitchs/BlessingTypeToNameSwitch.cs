using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars.Model;
using Localization;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(menuName = "Switch/BlessingTypeToNameSwitch")]
    public class BlessingTypeToNameSwitch : ScriptableObject
    {
        [SerializeField] private LanguageField<string> perun;
        [SerializeField] private LanguageField<string> svarog;
        [SerializeField] private LanguageField<string> health;
        [SerializeField] private LanguageField<string> stribog;
        [SerializeField] private LanguageField<string> acceleration;
        [SerializeField] private LanguageField<string> slowdown;
        [SerializeField] private LanguageField<string> power;
        [SerializeField] private LanguageField<string> spawn;
        [SerializeField] private LanguageField<string> gold;
        [SerializeField] private LanguageField<string> random;


        public string GetName(BlessingType type)
        {
            return type switch
            {
                BlessingType.None => "None",
                BlessingType.Perun => perun,
                BlessingType.Svarog => svarog,
                BlessingType.Health => health,
                BlessingType.Stribog => stribog,
                BlessingType.Acceleration => acceleration,
                BlessingType.Slowdown => slowdown,
                BlessingType.Power => power,
                BlessingType.Spawn => spawn,
                BlessingType.Gold => gold,
                BlessingType.Random => random,
                _ => "Если ты видишь это - ты уволен"
            };
        }
    }
}