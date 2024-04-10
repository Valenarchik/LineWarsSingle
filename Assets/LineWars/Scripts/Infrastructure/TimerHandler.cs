using System;
using System.Collections;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Infrastructure
{
    public abstract class TimerHandler: MonoBehaviour
    {
        [SerializeField] private string timerId;
        [SerializeField, Min(0)] protected float timeInSeconds;

        private string TimerId => $"timer_{timerId}";
        private bool started;
        public static GameRoot GameRoot => GameRoot.Instance;
        public static UserInfoController UserInfoController => GameRoot?.UserController;
        public static SDKAdapter SDKAdapter => GameRoot?.SdkAdapter;
        
        private void Start()
        {
            if (GameRoot != null)
            {
                if (GameRoot.GameReady)
                    StartTimer();
                else
                    GameRoot.OnGameReady += StartTimer;
            }
        }
        private void StartTimer()
        {
            if (SDKAdapter == null || UserInfoController == null)
                return;
            
            if (string.IsNullOrEmpty(TimerId) || timeInSeconds < 0)
                return;
            
            
            var currenTime = DateTime.Now;
            
            if (UserInfoController.KeyToDateTime.ContainsKey(TimerId))
            {
                var startTime = UserInfoController.KeyToDateTime[TimerId];

                if (currenTime < startTime) // кто-то игрался с системным временем?
                {
                    UserInfoController.KeyToDateTime[TimerId] = currenTime;
                    StartTime(timeInSeconds);
                    return;
                }
                
                var time = (currenTime - startTime).TotalSeconds.ToSingle();
                if (time <= timeInSeconds)
                    StartTime(timeInSeconds - time);
            }
            else
            {
                UserInfoController.KeyToDateTime[TimerId] = currenTime;
                StartTime(timeInSeconds);
            }
        }

        private void StartTime(float time)
        {
            if (!started)
                StartCoroutine(TimeCoroutine(time));
        }
        
        private IEnumerator TimeCoroutine(float time)
        {
            started = true;
            yield return new WaitForSeconds(time);

            started = false;
            TimeOut();
        }

        protected abstract void TimeOut();
    }
}