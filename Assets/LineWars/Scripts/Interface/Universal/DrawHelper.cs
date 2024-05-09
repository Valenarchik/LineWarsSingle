using System;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

public static class DrawHelper
{
    private static DrawHelperInstance Instance => GameRoot.Instance.DrawHelper;
    public static IReadOnlyDictionary<LootType, Sprite> LootTypeToSprite => Instance.LootTypeToSprite;
    public static IReadOnlyDictionary<Rarity, Color> RarityToColor => Instance.RarityToColor;
    public static IReadOnlyDictionary<Rarity, GameObject> RarityToShopCardBg => Instance.RarityToShopCardBg;
    public static IReadOnlyDictionary<GradientType, Gradient> TypeToGradient => Instance.TypeToGradient;
    public static IReadOnlyDictionary<LootType, Color> LootTypeToColor => Instance.LootTypeToColor;
    public static Sprite GoldSprite => Instance.GoldSprite;
    public static AllRefereeInfo AllRefereeInfo => Instance.AllRefereeInfo;
    
    public static FullBlessingReDrawInfo GetBlessingReDrawInfoByBlessingId(BlessingId blessingId)
    {
        return new FullBlessingReDrawInfo
        (
            GetBlessingNameByBlessingID(blessingId),
            GetBlessingDescription(blessingId),
            GetBlessingBgColor(blessingId),
            GetSpriteByBlessingID(blessingId),
            GameRoot.Instance.UserController.GlobalBlessingsPull[blessingId]
        );
    }

    public static Color GetBlessingBgColor(BlessingId blessingId)
    {
        return RarityToColor.TryGetValue(blessingId.Rarity, out var color) 
            ? color 
            : Color.white;
    }
    
    public static Sprite GetSpriteByBlessingID(BlessingId blessingId)
    {
        return GameRoot.Instance.BlessingStorage.TryGetValue(blessingId, out var value)
            ? value.Icon
            : null;
    }

    public static string GetBlessingTypeName(BlessingType blessingType)
    {
        return Instance.BlessingToName.GetName(blessingType);
    }
    
    public static string GetBlessingNameByBlessingID(BlessingId blessingId)
    {
        return GetBlessingTypeName(blessingId.BlessingType);
    }
    public static string GetBlessingDescription(BlessingId blessingId)
    {
        return GameRoot.Instance.BlessingStorage.TryGetValue(blessingId, out var value)
            ? value.Description
            : "Привет из мира багов";
    }
    
    public static Sprite GetSpriteByMissionStatus(MissionStatus missionStatus)
    {
        if (Instance.MissionStatusToIcon.TryGetValue(missionStatus, out var sprite))
            return sprite;
        return null;
    }
    
    public static Sprite GetOrderIconByCommandType(CommandType commandType)
    {
        if (Instance.CommandTypeToIcon.TryGetValue(commandType, out var info))
            return info.orderIcon;
        return null;
    }
    
    public static Sprite GetIconByCommandType(CommandType commandType)
    {
        if (Instance.CommandTypeToIcon.TryGetValue(commandType, out var info))
            return info.icon;
        return null;
    }

    public static Sprite GetOrderIconEffectType(EffectType effectType)
    {
        if (Instance.EffectToInfo.TryGetValue(effectType, out var info))
            return info.orderIcon;
        return null;
    }

    public static ActionOrEffectReDrawInfo GetReDrawInfoByEffectType(EffectType effectType)
    {
        var data = Instance.EffectToInfo;
        if (data.TryGetValue(effectType, out var value))
        {
            return new ActionOrEffectReDrawInfo(
                value.icon,
                value.name,
                value.description);
        }

        return null;
    }
    
    public static ActionOrEffectReDrawInfo GetReDrawInfoByCommandType(CommandType commandType)
    {
        var data = Instance.CommandTypeToIcon;
        if (data.TryGetValue(commandType, out var value))
        {
            return new ActionOrEffectReDrawInfo(
                value.icon,
                value.name,
                value.description
            );
        }

        return null;
    }
    
    public static string GetOnMissionButtonTextByMissionStatus(MissionStatus missionStatus)
    {
        return Instance.OnMissionButtonTextByMissionStatusSwitch.GetName(missionStatus);
    }
}