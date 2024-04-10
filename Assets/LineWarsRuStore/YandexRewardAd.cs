using System;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace LineWars.Controllers
{
    public class YandexRewardAd
    {
        private string adUnitId;
        private RewardedAdLoader rewardedAdLoader;
        private RewardedAd rewardedAd;
        public Action<object, Reward> Rewarded;

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
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            rewardedAdLoader.LoadAd(adRequestConfiguration);
        }

        public void ShowRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Show();
            }
        }

        private void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            // The ad was loaded successfully. Now you can handle it.
            rewardedAd = args.RewardedAd;

            // Add events handlers for ad actions
            rewardedAd.OnAdClicked += HandleAdClicked;
            rewardedAd.OnAdShown += HandleAdShown;
            rewardedAd.OnAdFailedToShow += HandleAdFailedToShow;
            rewardedAd.OnAdImpression += HandleImpression;
            rewardedAd.OnAdDismissed += HandleAdDismissed;
            rewardedAd.OnRewarded += HandleRewarded;
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            // Ad {args.AdUnitId} failed for to load with {args.Message}
            // Attempting to load a new ad from the OnAdFailedToLoad event is strongly discouraged.
        }

        private void HandleAdDismissed(object sender, EventArgs args)
        {
            // Called when an ad is dismissed.

            // Clear resources after an ad dismissed.
            DestroyRewardedAd();

            // Now you can preload the next rewarded ad.
            RequestRewardedAd();
        }

        private void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            // Called when rewarded ad failed to show.

            // Clear resources after an ad dismissed.
            DestroyRewardedAd();

            // Now you can preload the next rewarded ad.
            RequestRewardedAd();
        }

        private void HandleAdClicked(object sender, EventArgs args)
        {
            // Called when a click is recorded for an ad.
        }

        private void HandleAdShown(object sender, EventArgs args)
        {
            // Called when an ad is shown.
        }

        private void HandleImpression(object sender, ImpressionData impressionData)
        {
            // Called when an impression is recorded for an ad.
        }

        private void HandleRewarded(object sender, Reward args)
        {
            Rewarded.Invoke(sender, args);
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

