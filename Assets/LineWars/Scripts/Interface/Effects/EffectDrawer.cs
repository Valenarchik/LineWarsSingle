using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class EffectDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject powerObjects;
        [SerializeField] private TextMeshProUGUI powerText;
        [SerializeField] private GameObject roundsObjects;
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private Image image;

        private Effect<Node, Edge, Unit> effect;

        public void DrawEffect(Effect<Node, Edge, Unit> effect)
        {
            this.effect = effect;
            if (effect.HasCharacteristic(EffectCharecteristicType.Power))
            {
                var power = effect.GetCharacteristic(EffectCharecteristicType.Power);
                powerObjects.SetActive(true);
                powerText.text = power.GetRoman();
                effect.CharacteristicsChanged += OnPowerChanged;
            }
            else
            {
                powerObjects.SetActive(false);
            }

            if (effect.HasCharacteristic(EffectCharecteristicType.Duration))
            {
                var duration = effect.GetCharacteristic(EffectCharecteristicType.Duration);
                roundsObjects.SetActive(true);
                roundText.text = effect.GetCharacteristic(EffectCharecteristicType.Duration).ToString();
                effect.CharacteristicsChanged += OnRoundsChanged;
            }
            else
            {
                roundsObjects.SetActive(false);
            }
            image.sprite = DrawHelper.GetOrderIconEffectType(effect.EffectType);
        }

        private void OnDestroy()
        {
            if (effect.HasCharacteristic(EffectCharecteristicType.Power))
            {
                effect.CharacteristicsChanged -= OnPowerChanged;
            }

            if (effect.HasCharacteristic(EffectCharecteristicType.Duration))
            {
                effect.CharacteristicsChanged -= OnRoundsChanged;
            }
        }

        private void OnRoundsChanged(Effect<Node, Edge, Unit> effect,
            EffectCharecteristicType type,
            int prevRounds,
            int currentRounds)
        {
            var durationType = EffectCharecteristicType.Duration;
            if (type != durationType) return;
            roundText.text = effect.GetCharacteristic(durationType).ToString();
        }

        private void OnPowerChanged(Effect<Node, Edge, Unit> effect, 
            EffectCharecteristicType type, 
            int prevRounds, 
            int currentRounds)
        {
            var powerType = EffectCharecteristicType.Power;
            if (type != powerType) return;
            powerText.text = effect.GetCharacteristic(powerType).GetRoman();
        }
    }
}
