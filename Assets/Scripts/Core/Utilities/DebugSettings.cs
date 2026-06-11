using System;
using UnityEngine;

namespace Core.Utilities
{
    public class DebugSettings : MonoBehaviour
    {
        [SerializeField] private bool _loggerOn;
        public static bool LoggerOn = true;

        private void Awake()
        {
            LoggerOn = _loggerOn;
        }
    }
}