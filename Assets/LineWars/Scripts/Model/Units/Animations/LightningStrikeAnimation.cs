using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public class LightningStrikeAnimation: UnitAnimation
    {
        [Header("Lightning Settings")]
        [SerializeField] private float lightningTimeInSeconds = 0.5f;
        [SerializeField] private GameObject lightningPrefab;
        
        public override void Execute(AnimationContext context)
        {
            StartAnimation();
            Strike(context);
        }
        
        private void Strike(AnimationContext context)
        {
            var lightningInstance = Instantiate(lightningPrefab, context.TargetPosition, Quaternion.identity);
            StartCoroutine(DestroyCoroutine());
            IEnumerator DestroyCoroutine()
            {
                yield return new WaitForSeconds(lightningTimeInSeconds);
                Destroy(lightningInstance);
                EndAnimation();
            }
        }
    }
}