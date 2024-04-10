using LineWars.Controllers;

namespace LineWars.Infrastructure
{
    public class LevelMetricaHandler: LevelEventsHandler
    {
        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;
        
        protected override void LevelStarted()
        {
            if (SDKAdapter != null)
                SDKAdapter.SendStartLevelMetrica(Scene);
        }

        protected override void LevelFinished(LevelFinishStatus levelFinishStatus)
        {
            if (SDKAdapter != null)
            {
                SDKAdapter.SendFinishLevelMetrica(Scene, levelFinishStatus, SingleGameRoot.Instance.CurrentRound);
                if (levelFinishStatus != LevelFinishStatus.Exit)
                    SDKAdapter.SendFinishLevelMetrica(Scene, LevelFinishStatus.Exit);
            }
        }
    }
}