using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class WinOrLosePanel: MonoBehaviour
    {
        [SerializeField] private OpenAdButton openAdButton;
        private SDKAdapter SDKAdapter => GameRoot.Instance.SdkAdapter;

        private void OnEnable()
        {
            if (openAdButton != null && openAdButton.Button != null)
            {
                openAdButton.Button.interactable = WinOrLoseScene.CanDoublicateGold;
                openAdButton.PrizeForAd = new Prize(PrizeType.Gold, WinOrLoseScene.GoldAmount);
                openAdButton.Button.onClick.AddListener(OpenAdButtonOnClick);
            }
        }
        
        private void OnDisable()
        {
            if (openAdButton != null && openAdButton.Button != null)
                openAdButton.Button.onClick.RemoveListener(OpenAdButtonOnClick);
        }

        private void OpenAdButtonOnClick()
        {  
            if (openAdButton != null && openAdButton.Button != null)
            {
                openAdButton.Button.interactable = false;
            }
        }
    }
}