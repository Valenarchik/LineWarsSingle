using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Infrastructure
{
    
    [RequireComponent(typeof(CommandsManager))]
    public class CommandsManagerMetricaListener: MonoBehaviour
    {
        private CommandsManager commandsManager;
        private static SDKAdapter SDKAdapter => GameRoot.Instance?.SdkAdapter;
        private void Awake()
        {
            commandsManager = GetComponent<CommandsManager>();
        }

        private void OnEnable()
        {
            commandsManager.BlessingStarted += OnUseBlessing;
        }

        private void OnDisable()
        {
            commandsManager.BlessingStarted -= OnUseBlessing;
        }
        
        private void OnUseBlessing(BlessingMassage massage)
        {
            if (massage == null)
                return;
            
            if (SDKAdapter != null)
            {
                SDKAdapter.SendUseBlessingMetrica(massage.Data);
            }
        }
    }
}