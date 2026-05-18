using System.Collections.Generic;
using System.Linq;
using Core.Events;
using Gameplay.Bootstrap;
using Gameplay.Events;
using Reflex.Attributes;
using UnityEngine;

namespace Core.Bootstrap
{
    [DefaultExecutionOrder(-1000)]
    public class GameEntryPoint : MonoBehaviour
    {
        public static bool IsGameReady = false;

        [Inject] private IEnumerable<IInitializable> _installed;
        [SerializeField] private BootstrapController _bootstrapController;
        private HashSet<IInitializable> _initializables = new();


        private void Awake()
        {
            FindInitializables();

            InitializeAll();

            _bootstrapController.Boot();
            EventBus.Raise(new GameReadyEvent());
            IsGameReady = true;
            print("Game is ready");
        }

        private void FindInitializables()
        {
            var scripts = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var script in scripts)
            {
                if (script is IInitializable initializable)
                {
                    _initializables.Add(initializable);
                }
            }

            _initializables = _initializables.Concat(_installed).ToHashSet();
        }

        private void InitializeAll()
        {
            foreach (var initializable in _initializables)
            {
                initializable.Init();
            }
        }
        
    }
}