using System.Collections;
using UnityEngine;

namespace LineWars.Controllers
{
    [CreateAssetMenu(fileName = "new Single Repeat Music Logic", menuName = "Audio/Plugin/Music Logic/Single Repeat")]
    public class PluginSingleRepeatMusicLogicData : PluginMusicLogicData
    {
        [SerializeField] private PluginMusicData musicData; 
        [SerializeField] private float pauseTime;
        public override PluginMusicLogic GetMusicLogic(PluginMusicManager manager)
        {
            return new PluginSingleRepeatMusicLogic(manager, this);
        }

        public PluginMusicData MusicData => musicData;
        public float PauseTime => pauseTime;
    }

    public class PluginSingleRepeatMusicLogic : PluginMusicLogic
    {
        private readonly PluginSingleRepeatMusicLogicData data;
        private Coroutine musicCoroutine;

        public PluginSingleRepeatMusicLogic(PluginMusicManager manager, PluginSingleRepeatMusicLogicData data) : base(manager)
        {
            this.data = data;
        }

        public override void Start()
        {
            manager.Source.Loop = true;
            musicCoroutine = manager.StartCoroutine(MusicCoroutine());
        }

        public override void Exit()
        { 
            if (musicCoroutine != null)
                manager.StopCoroutine(musicCoroutine);
            musicCoroutine = null;

            manager.Source.Loop = false;
            manager.Stop();
        }

        private IEnumerator MusicCoroutine()
        {
            yield return new WaitForSeconds(data.PauseTime);
            manager.Play(data.MusicData);
        }
    }
}