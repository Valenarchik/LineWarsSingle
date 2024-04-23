using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Model;
using TMPro;
using UnityEngine;

public class LevelInfoPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text typeName;
    [SerializeField] private TMP_Text typeDescription;

    public void ReDraw(GameReferee gameReferee)
    {
        switch (gameReferee)
        {
            case CaptureThePointsGameReferee captureThePointsGameReferee:
                typeName.text = "Режим захват точек";
                typeDescription.text =
                    "Для победы нужно удерживать ключевые точки, они обведены желтым. За контроль каждой точки вы будете получать одно победное очко" +
                    $"\n \nЧтобы победить наберите {captureThePointsGameReferee.ScoreForWin}" +
                    $" победных {captureThePointsGameReferee.ScoreForWin.Pluralize("очко","очка","очков")}";
                break;
            case NewDominationGameReferee newDominationGameReferee:
                typeName.text = "Режим доминация";
                typeDescription.text = 
                    $"Для победы нужно захватить больше точек чем ящеры и удерживать их. Игра длится " +
                    $"{newDominationGameReferee.RoundsForWin} {newDominationGameReferee.RoundsForWin.Pluralize("раунд","раунда","раундов")}" +
                    $"\n \nВы победите, если на момент последнего раунда у вас будет больше точек, чем у ящеров";
                break;
            case KingOfMountainScoreReferee kingOfMountainScoreReferee:
                typeName.text = "Режим царь горы";
                typeDescription.text = 
                    $"На карте есть точка горы, она обведена желтым цветом. За каждый раунд, который вы контроллируете гору вам достается одно очко." +
                    $"\n \nЧтобы победить удерживайте контроль над горой " +
                    $"{kingOfMountainScoreReferee.ScoreForWin} {kingOfMountainScoreReferee.ScoreForWin.Pluralize("раунд","раунда","раундов")}";
                break;
            case MapCaptureScoreReferee mapCaptureScoreReferee:
                typeName.text = "Режим захват карты";
                typeDescription.text =
                    $"Чтобы победить нужно захватить " +
                    $"{mapCaptureScoreReferee.ScoreForWin} {mapCaptureScoreReferee.ScoreForWin.Pluralize("точка","точки", "точек")}" +
                    $" раньше, чем это сделают ящеры.";
                break;
            case SiegeGameReferee siegeGameReferee:
                typeName.text = "Режим осада";
                typeDescription.text =
                    $"Чтобы победить нужно продержаться " +
                    $"{siegeGameReferee.RoundsToWin} {siegeGameReferee.RoundsToWin.Pluralize("раунд", "раунда", "раундов")}" +
                    $"\n \nВ этом режиме вы не можете покупать юнитов во время боя, их нужно выбрать заранее.";
                break;
            case WallToWallGameReferee wallToWallGameReferee:
                typeName.text = "Режим заруба";
                typeDescription.text =
                    $"Чтобы победить нужно убить всех ящеров.\n \nВ этом режиме ни вы, ни ящеры не можете получать новых юнитов во время боя, вам нужно выбрать их заранее";
                break;
        }
    }
}
