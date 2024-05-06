using System;
using UnityEngine;

namespace LineWars.Education
{
    public abstract class EducationCardBase: MonoBehaviour
    {
        [SerializeField, HideInInspector] protected TransformCarousel carousel;

        private void OnValidate()
        {
            carousel ??= GetComponentInParent<TransformCarousel>();
        }
    }
}