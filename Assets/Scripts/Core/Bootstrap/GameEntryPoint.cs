using System.Collections.Generic;
using System.Reflection;
using Core.DI;
using UnityEngine;

namespace Core.Bootstrap
{
    [DefaultExecutionOrder(-999)]
    public class GameEntryPoint : MonoBehaviour
    {
        public static bool IsGameReady = false;
        [SerializeField] private Services _services;
        private readonly DIContainer _diContainer = new();

        private List<IInjectable> _injectables = new();
        private List<IInitializable> _initializables = new();


        private void Awake()
        {
            RegisterServices();

            FindAndSortScripts();
            
            InjectAll();
            
            InitializeAll();
            
            IsGameReady = true;
            print("Game is ready");
        }

        private void FindAndSortScripts()
        {
            var scripts = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var script in scripts)
            {
                if (script is IInjectable injectable)
                {
                    _injectables.Add(injectable);
                }
                else if (script is IInitializable initializable)
                {
                    _initializables.Add(initializable);
                }
            }
        }

        private void InjectAll()
        {
            foreach (var injectable in _injectables)
            {
                _diContainer.Inject(injectable);
            }
        }

        private void InitializeAll()
        {
            foreach (var initializable in _initializables)
            {
                initializable.Init();
            }
        }


        private void RegisterServices()
        {
            var type = _services.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fieldInfo in fields)
            {
                var serviceInstance = fieldInfo.GetValue(_services);
                if (serviceInstance == null) continue;
                _diContainer.Register(fieldInfo.FieldType, serviceInstance);
            }
        }
    }
}