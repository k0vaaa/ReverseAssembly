using UnityEngine;

namespace Core.Utilities
{
    public static class Utils
    {
        public static float ToDBf(float normalizedValue)
        {
            if (normalizedValue <= 0) normalizedValue = 0.0001f;
            
            normalizedValue = Mathf.Pow(normalizedValue, 2);
            
            return Mathf.Log10(normalizedValue) * 20;
        }
        public static float FromDBf(float dbf)
        {
            return Mathf.Sqrt(Mathf.Pow(10, dbf / 20));
            
        }
    }
}