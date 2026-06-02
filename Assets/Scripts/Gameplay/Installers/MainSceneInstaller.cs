using Core.Inventory;
using Core.UI;
using Gameplay.Core;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using Resolution = Reflex.Enums.Resolution;

namespace Gameplay.Installers
{
    public class MainSceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Window _mainWindow;
        [SerializeField] private WindowManager _windowManager;
        [SerializeField] private BranchManager _branchManager;
        [SerializeField] private Volume _postProcessVolume;
        [SerializeField] private InventoryManager _inventoryManager;
        private AnimationClip _clip;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(_mainWindow, new []{typeof(Window)});
            builder.RegisterValue(_windowManager);
            builder.RegisterValue(_branchManager);
            builder.RegisterValue(_postProcessVolume);
            builder.RegisterValue(_inventoryManager);
            builder.RegisterType(typeof(SyncEnergyManager), Lifetime.Singleton, Resolution.Eager);
        }
    }
}