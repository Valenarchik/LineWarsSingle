using System;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace LineWars.Controllers
{
    public class YandexRewardAd
    {
        private readonly string adUnitId;
        private RewardedAdLoader rewardedAdLoader;
        private RewardedAd rewardedAd;

        private Action onSuccess;
        private Action onFailure;

        public YandexRewardAd(string adUnitId)
        {
            this.adUnitId = adUnitId;   
            
            SetupLoader();
            RequestRewardedAd();
        }

        private void SetupLoader()
        {
            rewardedAdLoader = new RewardedAdLoader();
            rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
            rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
        }

        private void RequestRewardedAd()
        {
            var adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            rewardedAdLoader.LoadAd(adRequestConfiguration);
        }

        public void ShowRewardedAd(Action onSuccess, Action onFailure)
        {
            if (rewardedAd != null)
            {
                this.onSuccess = onSuccess;
                this.onFailure = onFailure;
                
                rewardedAd.Show();
            }
        }

        private void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            rewardedAd = args.RewardedAd;

            rewardedAd.OnAdClicked += HandleAdClicked;
            rewardedAd.OnAdShown += HandleAdShown;
            rewardedAd.OnAdFailedToShow += HandleAdFailedToShow;
            rewardedAd.OnAdImpression += HandleImpression;
            rewardedAd.OnAdDismissed += HandleAdDismissed;
            rewardedAd.OnRewarded += HandleRewarded;
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
        }

        private void HandleAdDismissed(object sender, EventArgs args)
        {
            DestroyRewardedAd();
            RequestRewardedAd();
            onFailure?.Invoke();
        }

        private void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            DestroyRewardedAd();
            RequestRewardedAd();
            onFailure?.Invoke();
        }

        private void HandleAdClicked(object sender, EventArgs args)
        {
        }

        private void HandleAdShown(object sender, EventArgs args)
        {
        }

        private void HandleImpression(object sender, ImpressionData impressionData)
        {
        }

        private void HandleRewarded(object sender, Reward args)
        {
            onSuccess?.Invoke();
        }

        private void DestroyRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
        }
    }
}

