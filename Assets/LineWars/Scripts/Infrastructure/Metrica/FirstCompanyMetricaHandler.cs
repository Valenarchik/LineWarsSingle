using LineWars.Controllers;

namespace LineWars.Infrastructure
{
    public class FirstCompanyMetricaHandler: LevelEventsHandler
    {
        private const string Id = "firstCompany";
        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;
        private static UserInfoController UserController => GameRoot.Instance?.UserController;
        
        protected override void LevelStarted()
        {
            if (SDKAdapter != null && UserController != null && !UserController.KeyToBool[Id])
            {
                UserController.KeyToBool[Id] = true;
                SDKAdapter.SendFirstCompanyMetrica();
            }
        }

        protected override void LevelFinished(LevelFinishStatus levelFinishStatus)
        {
        }
    }
}