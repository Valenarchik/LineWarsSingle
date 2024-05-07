using UnityEngine;

namespace Localization
{
    internal sealed class ConditionallyVisibleAttribute: PropertyAttribute
    {
        public string propertyName { get; }
        public bool invert { get; }
        
        public ConditionallyVisibleAttribute(string propertyName, bool invert = false)
        {
            this.propertyName = propertyName;
            this.invert = invert;
        }
    }
}