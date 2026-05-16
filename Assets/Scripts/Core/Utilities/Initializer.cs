using System.Reflection;
using Core.Bootstrap;
using UnityEngine;

namespace Core.Utilities
{
    public static class Initializer
    {
        public static void InitializeScript<T>(T obj) where T : class
        {
            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                if (field.GetValue(obj) is IInitializable initializable)
                {
                    initializable?.Init();
                    Debug.Log($"{initializable.GetType().Name} initialized");
                    
                }
            }
        }

        /// <summary>
        /// Initializes all fields in all components of passed GameObject
        /// </summary>
        /// <param name="go"></param>
        public static void InitializeGO(GameObject go)
        {
            var initializables = go.GetComponents<MonoBehaviour>();
            foreach (var initializable in initializables)
            {
                Debug.Log($"{initializable.GetType().Name} initialized");
                InitializeScript(initializable);
            }
        }
    }
}