using LineWars;
using LineWars.Model;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;
        
        // Ваши сохранения
        public UserInfo UserInfo;
        public DeckInfo[] Decks;
        public MissionInfo[] Missions;
    }
}
