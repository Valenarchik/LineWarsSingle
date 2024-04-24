using UnityEngine;

namespace Localization
{
    public class LanguageTextAreaAttribute: PropertyAttribute
    {
        public readonly float Height;

        public LanguageTextAreaAttribute(float height = 60)
        {
            Height = height;
        }
    }
}