using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Infrastructure
{
    public class EndlessLevelMetricaHandler: LevelEventsHandler
    {
        [SerializeField] private InfinityGame infinityGame;
        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;
        protected override void LevelStarted()
        {
            if (SDKAdapter != null && infinityGame.CurrentMode != null)
            { 
                SDKAdapter.SendStartEndlessLevelMetrica(Scene, infinityGame.CurrentMode.Value);
            }
        }

        protected override void LevelFinished(LevelFinishStatus levelFinishStatus)
        {
            if (SDKAdapter != null && infinityGame.CurrentMode != null)
            {
                SDKAdapter.SendFinishEndlessLevelMetrica(Scene, infinityGame.CurrentMode.Value, levelFinishStatus);
                if (levelFinishStatus != LevelFinishStatus.Exit)
                    SDKAdapter.SendFinishEndlessLevelMetrica(Scene, infinityGame.CurrentMode.Value, LevelFinishStatus.Exit);
            }
        }
    }
}