using System;
using System.Collections;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Button))]
    public class TimerOpenAddButton: MonoBehaviour
    {
        private static SDKAdapter SdkAdapter => GameRoot.Instance.SdkAdapter;
        private static UserInfoController UserController => GameRoot.Instance.UserController;
        private static IGetter<DateTime> TimeGetter => GameRoot.Instance.TimeGetter;
        
        [SerializeField] private float timeInMinutes;
        [SerializeField] private string keyID;
        [SerializeField] private Prize prizeForAd;
        [SerializeField] private TMP_Text timerText;
        public Prize PrizeForAd
        {
            get => prizeForAd;
            set => prizeForAd = value;
        }
        
        private Button button;
        public Button Button => button;
        
        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void FixedUpdate()
        {
            if(timerText != null)
                timerText.text = GetRemainingTimeString();
        }

        private void OnEnable()
        {
            var checkTime = CheckTime();
            button.interactable = checkTime;
            SdkAdapter.RewardVideoEvent += SdkAdapterOnRewardVideoEvent;
            button.onClick.AddListener(OnClick);

            if (!checkTime)
                StartCoroutine(WaitingCoroutine());
        }
        
        private void OnDisable()
        {
            SdkAdapter.RewardVideoEvent -= SdkAdapterOnRewardVideoEvent;
            button?.onClick?.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (SdkAdapter != null)
                SdkAdapter.RewardForAd(prizeForAd);
        }

        private IEnumerator WaitingCoroutine()
        {
            var totalSeconds = GetTimeRemainingTimeInSeconds();
            yield return new WaitForSeconds(totalSeconds);
            button.interactable = true;
        }
        
        private void SdkAdapterOnRewardVideoEvent(PrizeType prizeType, int amount)
        {
            if (PrizeForAd.Type == prizeType && PrizeForAd.Amount == amount)
            {
                button.interactable = false;
                UpdateTime();
                StartCoroutine(WaitingCoroutine());
            }
        }
        
        private bool CheckTime()
        {
            if (!TimeGetter.CanGet())
                return false;
            if (!UserController.KeyToDateTime.ContainsKey(keyID))
                return true;
            var time = TimeGetter.Get();
            var prevTime = UserController.KeyToDateTime[keyID];
            if (prevTime > time)
                return true;
            var totalMinutes = (float)(time - prevTime).TotalMinutes;
            return totalMinutes > timeInMinutes;
        }

        private float GetTimeRemainingTimeInSeconds()
        {
            if(!UserController.KeyToDateTime.ContainsKey((keyID)))
                return 0f;
            var time = TimeGetter.Get();
            var prevTime = UserController.KeyToDateTime[keyID];
            var totalSecond = (float)(time - prevTime).TotalSeconds;
            return timeInMinutes * 60 - totalSecond;
        }

        private string GetRemainingTimeString()
        {
            var seconds = GetTimeRemainingTimeInSeconds();
            var (min, sec) = (seconds / 60, seconds % 60);
            return $"{min.ToChars(2)} : {sec.ToChars(2)}";
        }
        
        private void UpdateTime()
        {
            if (!TimeGetter.CanGet())
                return;
            var time = TimeGetter.Get();
            UserController.KeyToDateTime[keyID] = time;
        }
    }
}