using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Infrastructure
{
    [DefaultExecutionOrder(121)]
    public class FirstPurchaseMetricaHandler: MonoBehaviour
    {
        private const string Id = "firstPurchase";
        public UserInfoController UserInfoController => GameRoot.Instance?.UserController;
        private SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;

        
        private void Start()
        {
            SDKAdapter.PurchaseSuccessEvent += SDKAdapterOnPurchaseSuccessEvent;
        }
        
        private void OnDestroy()
        {
            if (SDKAdapter != null)
            {
                SDKAdapter.PurchaseSuccessEvent -= SDKAdapterOnPurchaseSuccessEvent;
            }
        }

        private void SDKAdapterOnPurchaseSuccessEvent(string id)
        {
            if (UserInfoController != null && !UserInfoController.KeyToBool[Id])
            {
                UserInfoController.KeyToBool[Id] = true;
                SDKAdapter.SendFirstPurchaseMetrica();
            }
        }
    }
}