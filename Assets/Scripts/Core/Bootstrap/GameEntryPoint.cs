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

        private HashSet<IInjectable> _injectables = new();
        private HashSet<IInitializable> _initializables = new();


        private void Awake()
        {
            _diContainer.Register(_diContainer);
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
                if (script is IInitializable initializable)
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
                if (serviceInstance is IInitializable initializable)
                {
                    _initializables.Add(initializable);
                }
                if (serviceInstance is IInjectable injectable)
                {
                    _injectables.Add(injectable);
                }
            }
            
        }
    }
}