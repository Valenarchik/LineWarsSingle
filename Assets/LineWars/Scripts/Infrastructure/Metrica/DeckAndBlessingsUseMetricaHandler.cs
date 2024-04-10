using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LineWars.Infrastructure
{
    public class DeckAndBlessingsUseMetricaHandler: MonoBehaviour
    {
        private DecksController DecksController => GameRoot.Instance?.DecksController;
        private SDKAdapter SdkAdapter => GameRoot.Instance?.SdkAdapter;
        private BlessingsController BlessingsController => GameRoot.Instance?.BlessingsController;
        
        
        private void Start()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
        }
        
        private void SceneManagerOnActiveSceneChanged(Scene current, Scene next)
        {
            var nextScene = (SceneName) next.buildIndex;
            
            if (nextScene != SceneName.MainMenu && nextScene != SceneName.WinOrLoseScene) // переход на другую сцену
            {
                if (SdkAdapter != null 
                    && DecksController != null 
                    && DecksController.DeckToGame != null)
                {
                    SdkAdapter.SendDeckMetrica(DecksController.DeckToGame);
                }

                if (SdkAdapter != null
                    && BlessingsController != null)
                {
                    var pullForGame = BlessingsController.BlissingPullForGame;
                    SdkAdapter.SendBlessingsMetrica(pullForGame);
                }
            }
        }
    }
}