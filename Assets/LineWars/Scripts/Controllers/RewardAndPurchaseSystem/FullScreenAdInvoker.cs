using System.Collections.Generic;
using LineWars.Controllers;
using UnityEngine;

public class FullScreenAdInvoker: MonoBehaviour
{
    [SerializeField] private List<SceneName> ignoreScenes;
    private static SDKAdapter SDKAdapter => GameRoot.Instance.SdkAdapter;
    private void OnEnable()
    {
        SceneTransition.StartLoadScene += SceneTransitionOnStartLoadScene;
    }
    
    private void OnDisable()
    {
        SceneTransition.StartLoadScene -= SceneTransitionOnStartLoadScene;
    }

    private void SceneTransitionOnStartLoadScene(SceneName scene)
    {
        if (ignoreScenes.Contains(scene))
            return;
        SDKAdapter.FullscreenShow();
    }
}
