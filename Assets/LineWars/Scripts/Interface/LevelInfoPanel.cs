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
                typeName.text = DrawHelper.AllRefereeInfo.CaptureThePoints.Name;
                typeDescription.text = string.Format(
                    DrawHelper.AllRefereeInfo.CaptureThePoints.Description,
                    captureThePointsGameReferee.ScoreForWin);
                break;
            case NewDominationGameReferee newDominationGameReferee:
                typeName.text = DrawHelper.AllRefereeInfo.NewDomination.Name;
                typeDescription.text = string.Format(
                    DrawHelper.AllRefereeInfo.NewDomination.Description,
                    newDominationGameReferee.RoundsForWin);
                break;
            case KingOfMountainScoreReferee kingOfMountainScoreReferee:
                typeName.text = DrawHelper.AllRefereeInfo.KingOfMountain.Name;
                typeDescription.text = string.Format(
                    DrawHelper.AllRefereeInfo.KingOfMountain.Description,
                    kingOfMountainScoreReferee.ScoreForWin);
                break;
            case MapCaptureScoreReferee mapCaptureScoreReferee:
                typeName.text = DrawHelper.AllRefereeInfo.MapCapture.Name;
                typeDescription.text = string.Format(
                    DrawHelper.AllRefereeInfo.MapCapture.Description,
                    mapCaptureScoreReferee.ScoreForWin);
                break;
            case SiegeGameReferee siegeGameReferee:
                typeName.text = DrawHelper.AllRefereeInfo.Siege.Name;
                typeDescription.text = string.Format(
                    DrawHelper.AllRefereeInfo.Siege.Description,
                    siegeGameReferee.RoundsToWin);
                break;
            case WallToWallGameReferee wallToWallGameReferee:
                typeName.text = DrawHelper.AllRefereeInfo.WallToWall.Name;
                typeDescription.text = string.Format(
                    DrawHelper.AllRefereeInfo.WallToWall.Description
                    );
                break;
        }
    }
}
