using UnityEngine;

namespace Assets.PropertyAttributes
{
    public class RangeDouble : PropertyAttribute
    {
        public double Min;
        public double Max;

        public RangeDouble(double min, double max)
        {
            Min = min;
            Max = max;
        }
    }
}
