using UnityEngine;

namespace Core.Extensions
{
    public static class TransformExtensions
    {
        public static void ResetPosition(this Transform transform)
        {
            transform.position = Vector3.zero;
        }

        public static void ResetRotation(this Transform transform)
        {
            transform.rotation = Quaternion.identity;
        }
        
        public static void ResetScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }
        
        public static void ResetAll(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        
        public static void LocalResetPosition(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        public static void LocalResetScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }

        public static void LocalResetRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }
        
        public static void LocalResetAll(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}