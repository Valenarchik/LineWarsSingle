using System.Collections;
using System.Collections.Generic;
using Localization;
using UnityEngine;

namespace LineWars
{
    [CreateAssetMenu(fileName = "New Music Data", menuName = "Audio/Plugin/Music Logic/Music Data")]
    public class PluginMusicData : ScriptableObject
    {
        [SerializeField] private string audioClipKey;
        [SerializeField] private LanguageField<string> languageSongName;
        [SerializeField] private LanguageField<string> languageArtist;
        [SerializeField, Range(0, 1)] private float volumeCoeficient;
        [SerializeField] private bool isAuthorMusic;

        public string AudioClipKey => audioClipKey;
        public string Name => languageSongName;
        public string Artist => languageArtist;
        public float VolumeCoeficient => volumeCoeficient;
        public bool IsAuthorMusic => isAuthorMusic;
    }
}
