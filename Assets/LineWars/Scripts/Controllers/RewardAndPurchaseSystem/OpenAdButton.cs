using System;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Button))]
    [DisallowMultipleComponent]
    public class OpenAdButton: MonoBehaviour
    {
        private static SDKAdapter SdkAdapter => GameRoot.Instance.SdkAdapter;
        
        [SerializeField] private Prize prizeForAd;
        private Button button;

        public Prize PrizeForAd
        {
            set => prizeForAd = value;
        }

        public Button Button => button;
        
        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button?.onClick?.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (SdkAdapter != null)
                SdkAdapter.RewardForAd(prizeForAd);
        }
    }
}