using System;
using UnityEngine;

namespace Core.Utilities
{
    public class Logger
    {
        public static void LogTyped(Type type, string message)
        {
            Debug.Log($"{type.Name}: {message}");
        }
    }
}