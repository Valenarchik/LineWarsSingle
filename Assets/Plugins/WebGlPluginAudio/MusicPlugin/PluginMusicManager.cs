using Plugins.Audio.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(SourceAudio))]
    public class PluginMusicManager : MonoBehaviour
    {
        public static PluginMusicManager Instance { get; private set; }
        [SerializeField] private PluginMusicLogicData musicLogicData;
        [SerializeField] private TwoStringEventChannel raisedMusicChangedChannel;
        
        [Header("Fade Out Settings")]
        [SerializeField] private AnimationCurve fadeOutCurve;
        [SerializeField] private float fadeOutTime;

        [Header("Fade In Settings")]
        [SerializeField] private AnimationCurve fadeInCurve;
        [SerializeField] private float fadeInTime;

        private SourceAudio source;
        private PluginMusicLogic logic;
        private float initialVolume;
        public PluginMusicData CurrentMusicData { get; private set; }
        public SourceAudio Source => source;
        public UnityEvent<PluginMusicData, PluginMusicData> MusicChanged;
        public UnityEvent<PluginMusicManager, PluginMusicData> MusicFinished;

        //  public AudioSource Source => source;
        public float Volume
        {
            get => source.Volume;
            set => source.Volume = value;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Instance.SwitchMusicLogic(musicLogicData);
                Destroy(gameObject);
                return;
            }

            source = GetComponent<SourceAudio>();
            logic = musicLogicData.GetMusicLogic(this);
            
        }

        private void Start()
        {
            initialVolume = source.Volume;
            logic.Start();
            source.OnFinished += InvokeMusicFinished;
        }


        private void Update()
        {
            logic.Update();
        }
        private void InvokeMusicFinished()
        {
            MusicFinished.Invoke(this, CurrentMusicData);
        }

        private void SwitchMusicLogic(PluginMusicLogicData data)
        {
            StartCoroutine(SwitchCoroutine());
            IEnumerator SwitchCoroutine()
            {
                var passedTime = 0f;
                while (passedTime < fadeOutTime)
                {
                    yield return null;
                    passedTime += Time.deltaTime;
                    source.Volume = fadeOutCurve.Evaluate(passedTime / fadeOutTime);
                }
                logic.Exit();
                logic = data.GetMusicLogic(this);
                logic.Start();
                
                passedTime = 0f;
                while (passedTime < fadeInTime)
                {
                    yield return null;
                    passedTime += Time.deltaTime;
                    source.Volume = fadeInCurve.Evaluate(passedTime / fadeOutTime);
                }
            }
        }

        public void Play(PluginMusicData musicData)
        {
            var prevData = CurrentMusicData;
            source.Volume = source.Volume * musicData.VolumeCoeficient;
            source.Play(musicData.AudioClipKey);
            CurrentMusicData = musicData;
            MusicChanged.Invoke(prevData, musicData);
            raisedMusicChangedChannel.RaiseEvent(musicData.Name, musicData.Artist);
        }

        public void Stop()
        {
            if(CurrentMusicData == null)
                return;
            var prevData = CurrentMusicData;
            CurrentMusicData = null;
            source.Stop();
            MusicChanged.Invoke(prevData, null);
        }
    }
}

