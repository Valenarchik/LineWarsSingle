using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Infrastructure
{
    public class Rewarder: MonoBehaviour
    {
        [SerializeField] private Trigger trigger;
        [SerializeField] private Prize prize;
        

        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;
        
        private void Awake()
        {
            if (trigger != null)
            {
                if (trigger.Triggered)
                    OnTriggered();
                else
                    trigger.TriggeredEvent += OnTriggered;
            }
        }

        private void OnTriggered()
        {
            if (SDKAdapter != null)
            {
                SDKAdapter.Reward(prize);
            }
        }
    }
}