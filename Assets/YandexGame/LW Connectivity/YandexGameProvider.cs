using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using YG;

namespace LineWars
{
    public class YandexGameProvider : ProviderBase
    {
        private bool isLoaded;
        private GameInfo gameInfo;

        public void LoadAll()
        {
            isLoaded = true;
            gameInfo = new GameInfo().UpdateInfo(YandexGame.savesData);
        }
        
        private void UpdateYGSave()
        {
            YandexGame.savesData = YandexGame.savesData.UpdateSave(gameInfo);
            YandexGame.SaveProgress();
        }

        public override void Save(MissionInfo value, int id)
        {
            gameInfo.Missions ??= new Dictionary<int, MissionInfo>();
            gameInfo.Missions[id] = value;
            UpdateYGSave();
        }

        public override void Save(DeckInfo value, int id)
        {
            gameInfo.Decks ??= new Dictionary<int, DeckInfo>();
            gameInfo.Decks[id] = value;
            UpdateYGSave();
        }

        public override void Save(UserInfo value, int id)
        {
            gameInfo.UserInfo = value;
            UpdateYGSave();
        }

        public override DeckInfo LoadDeckInfo(int id)
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            if (gameInfo.Decks == null || !gameInfo.Decks.ContainsKey(id))
                return null;
            return gameInfo.Decks[id];
        }

        public override IEnumerable<DeckInfo> LoadAllDeckInfo()
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            return gameInfo.Decks?.Values ?? Enumerable.Empty<DeckInfo>();
        }

        public override UserInfo LoadUserInfo(int id)
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            return gameInfo.UserInfo;
        }

        public override IEnumerable<UserInfo> LoadAllUserInfo()
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            if (gameInfo.UserInfo == null)
                yield break;
            yield return gameInfo.UserInfo;
        }

        public override MissionInfo LoadMissionInfo(int id)
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            if (gameInfo.Missions == null || !gameInfo.Missions.ContainsKey(id))
                return null;
            return gameInfo.Missions[id];
        }

        public override IEnumerable<MissionInfo> LoadAllMissionInfo()
        {
            if (!isLoaded)
                throw new InvalidOperationException();
            return gameInfo.Missions?.Values ?? Enumerable.Empty<MissionInfo>();
        }
    }
}