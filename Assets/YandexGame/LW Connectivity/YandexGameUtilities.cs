using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using YG;

namespace LineWars
{
    public static class YandexGameUtilities
    {
        // <----------------------------YG->Info------------------------------>
        public static GameInfo UpdateInfo(this GameInfo gameInfo, SavesYG savesYg)
        {
            if (savesYg == null)
            {
                Debug.LogError($"{nameof(savesYg)} is null!");
                return gameInfo;
            }

            gameInfo.Decks = savesYg.Decks
                ?.ToDictionary(deckInfo => deckInfo.Id, deckInfo => deckInfo);
            gameInfo.UserInfo = savesYg.UserInfo;
            gameInfo.Missions = savesYg.Missions
                ?.ToDictionary(missionInfo => missionInfo.MissionId, missionInfo => missionInfo);
            
            return gameInfo;
        }

        //<----------------------------Info->YG------------------------------>
        public static SavesYG UpdateSave(this SavesYG savesYg, GameInfo gameInfo)
        {
            if (gameInfo == null)
            {
                Debug.LogError($"{nameof(gameInfo)} is null!");
                return savesYg;
            }

            savesYg.Decks = gameInfo.Decks?.Select(pair => pair.Value).ToArray();
            savesYg.Missions = gameInfo.Missions?.Select(pair => pair.Value).ToArray();
            savesYg.UserInfo = gameInfo.UserInfo;
            
            return savesYg;
        }
    }
}