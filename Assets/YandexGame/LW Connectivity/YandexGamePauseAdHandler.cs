using LineWars;
using UnityEngine;
using YG;

public class YandexGamePauseAdHandler : MonoBehaviour
{
    private void OnEnable()
    {
        YandexGame.OpenFullAdEvent += OpenFullscreenAd;
        YandexGame.CloseFullAdEvent += CloseFullscreenAd;
        YandexGame.OpenVideoEvent += OpenRewardedAd;
        YandexGame.CloseVideoEvent += CloseRewardedAd;
    }
    
    private void OnDisable()
    {
        YandexGame.OpenFullAdEvent -= OpenFullscreenAd;
        YandexGame.CloseFullAdEvent -= CloseFullscreenAd;
        YandexGame.OpenVideoEvent -= OpenRewardedAd;
        YandexGame.CloseVideoEvent -= CloseRewardedAd;
    }
    
    private void CloseRewardedAd()
    {
        PauseInstaller.Pause(PauseType.All, false);
    }

    private void OpenRewardedAd()
    {
        PauseInstaller.Pause(PauseType.All, true);
    }

    private void CloseFullscreenAd()
    {
        PauseInstaller.Pause(PauseType.All, false);
    }

    private void OpenFullscreenAd()
    {
        PauseInstaller.Pause(PauseType.All, true);
    }
}
