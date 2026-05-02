using UnityEngine;

namespace Core.Extensions
{
    public static class FloatExtensions
    {
        public static float Round(this float num)
        {
            return Mathf.Round(num);
        }
    }
}