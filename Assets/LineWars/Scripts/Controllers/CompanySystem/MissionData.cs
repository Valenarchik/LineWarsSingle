using Localization;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New MissionData", menuName = "Data/Create MissionData", order = 50)]
    public class MissionData : ScriptableObject
    {
        [SerializeField] private LanguageField<string> langMissionName;
        [SerializeField, LanguageTextArea] private LanguageField<string> langMissionDescription;
        [SerializeField] private Sprite missionImage;
        [SerializeField] private StableEnum<SceneName> sceneToLoadStable;
        
        public string MissionName => langMissionName;
        public string MissionDescription => langMissionDescription;
        public Sprite MissionImage => missionImage;
        public SceneName SceneToLoad => sceneToLoadStable;
    }
}