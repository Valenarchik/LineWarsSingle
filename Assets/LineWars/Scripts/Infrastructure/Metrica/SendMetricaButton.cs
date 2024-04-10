using System;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Infrastructure
{
    [DisallowMultipleComponent]
    public class SendMetricaButton: ButtonClickHandler
    {
        [SerializeField] private string buttonId;
        [SerializeField] private bool listenFirstClick = true;
        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;
        private static UserInfoController UserInfoController => GameRoot.Instance?.UserController;
        protected override void OnClick()
        {
            if (string.IsNullOrEmpty(buttonId) 
                || SDKAdapter == null 
                || UserInfoController == null)
                return;
            
            SDKAdapter.SendButtonMetrica(buttonId);
            if (!listenFirstClick)
                return;

            var sent = UserInfoController.KeyToBool[buttonId];
            if (sent)
                return;
            SDKAdapter.SendFirstButtonMetrica(buttonId);
            UserInfoController.KeyToBool[buttonId] = true;
        }
    }
}