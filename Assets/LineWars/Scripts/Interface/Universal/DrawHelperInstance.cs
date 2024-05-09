using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars;
using LineWars.Model;
using Localization;
using UnityEngine;

public class DrawHelperInstance : MonoBehaviour
{
    [SerializeField] private Sprite goldSprite;
    [SerializeField] private BlessingTypeToNameSwitch blessingToName;
    [SerializeField] private MissionStatusToNameSwitch onMissionButtonTextByMissionStatusSwitch;
    [SerializeField] private AllRefereeInfo allRefereeInfo;
    
    [Header("Dictionaries")]
    [SerializeField] private SerializedDictionary<LootType, Color> lootTypeToColor;
    [SerializeField] private SerializedDictionary<LootType, Sprite> lootTypeToSprite;
    [SerializeField] private SerializedDictionary<Rarity, Color> rarityToColor;
    [SerializeField] private SerializedDictionary<MissionStatus, Sprite> missionStatusToIcon;
    [SerializeField] private SerializedDictionary<Rarity, GameObject> rarityToShopCardBg;
    [SerializeField] private SerializedDictionary<GradientType, Gradient> typeToGradient;

    [Header("Commands")] 
    [SerializeField] private SerializedDictionary<CommandType, EffectOrCommandInfo> commandTypeToIcon;

    [Header("Effects")]
    [SerializeField] private SerializedDictionary<EffectType, EffectOrCommandInfo> effectToInfo;
    
    public IReadOnlyDictionary<LootType, Color> LootTypeToColor => lootTypeToColor;
    public IReadOnlyDictionary<MissionStatus, Sprite> MissionStatusToIcon => missionStatusToIcon;
    public IReadOnlyDictionary<CommandType, EffectOrCommandInfo> CommandTypeToIcon => commandTypeToIcon;
    public IReadOnlyDictionary<LootType, Sprite> LootTypeToSprite => lootTypeToSprite;
    public IReadOnlyDictionary<Rarity, Color> RarityToColor => rarityToColor;
    public IReadOnlyDictionary<Rarity, GameObject> RarityToShopCardBg => rarityToShopCardBg;
    public IReadOnlyDictionary<GradientType, Gradient> TypeToGradient => typeToGradient;
    public IReadOnlyDictionary<EffectType, EffectOrCommandInfo> EffectToInfo => effectToInfo;

    public Sprite GoldSprite => goldSprite;
    
    public BlessingTypeToNameSwitch BlessingToName => blessingToName;
    
    public MissionStatusToNameSwitch OnMissionButtonTextByMissionStatusSwitch => onMissionButtonTextByMissionStatusSwitch;

    public AllRefereeInfo AllRefereeInfo => allRefereeInfo;
}

public enum GradientType
{
    Common,
    Epic,
    Gold,
    Upgrade,
    Diamond
}

[Serializable]
public class EffectOrCommandInfo
{
    public Sprite icon;
    public Sprite orderIcon;
    public LanguageField<string> name;
    public LanguageField<string> description;
}