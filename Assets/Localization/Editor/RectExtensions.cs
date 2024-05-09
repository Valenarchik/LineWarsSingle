using UnityEngine;

namespace Localization
{
    public static class RectExtensions
    {
        public static Rect AddX(this Rect rect, float value)
        {
            return new Rect(rect.x + value, rect.y, rect.width, rect.height);
        }
        
        public static Rect AddY(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y + value, rect.width, rect.height);
        }
        
        public static Rect AddWidth(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y, rect.width + value, rect.height);
        }
        
        public static Rect SubtractWidth(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y, rect.width - value, rect.height);
        }
        
        public static Rect AddHeight(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y, rect.width, rect.height + value);
        }
        
        public static Rect SubtractHeight(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y, rect.width, rect.height - value);
        }
        
        public static Rect SetX(this Rect rect, float value)
        {
            return new Rect(value, rect.y, rect.width, rect.height);
        }
        
        public static Rect SetY(this Rect rect, float value)
        {
            return new Rect(rect.x, value, rect.width, rect.height);
        }
        
        public static Rect SetWidth(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y,value, rect.height);
        }
        
        public static Rect SetHeight(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y,rect.width, value);
        }
        
        public static Rect RelativeX(this Rect rect, float value)
        {
            return new Rect(rect.x + rect.width * value, rect.y, rect.width, rect.height);
        }
        public static Rect WidthPercent(this Rect rect, float value)
        {
            return new Rect(rect.x, rect.y,rect.width * value, rect.height);
        }
    }
}