using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Core.Utilities
{
    public class Logger
    {
        public static void LogTyped(Type type, string message)
        {
            if (!DebugSettings.LoggerOn) return;
            Debug.Log($"{type.Name}: {message}");
        }
        
        public static void Trace([CallerMemberName] string methodName = "")
        {
            if (!DebugSettings.LoggerOn) return;
            Debug.Log($"{methodName} выполнен");
        }

        public static void LogTagged(string message, string tag = "Default")
        {
            if (!DebugSettings.LoggerOn) return;
            Debug.Log($"{tag.ToUpper()} | {message} | {tag.ToUpper()}");
        }
    }
}