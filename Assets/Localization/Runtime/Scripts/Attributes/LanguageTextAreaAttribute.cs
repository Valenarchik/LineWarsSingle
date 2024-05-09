using System;
using UnityEngine;

namespace Localization
{
    public class LanguageTextAreaAttribute: PropertyAttribute
    {
        public readonly float MinHeight;
        public readonly float MaxHeight;

        public LanguageTextAreaAttribute(float minHeight = 60, float maxHeight = 500)
        {
            if (minHeight < 0 || maxHeight < 0 || minHeight >= maxHeight)
                throw new ArgumentException();
            
            MaxHeight = maxHeight;
            MinHeight = minHeight;
        }
    }
}