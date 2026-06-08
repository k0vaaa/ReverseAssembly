using Core.Inventory;
using Core.UI;
using Gameplay.Core;
using Gameplay.UI.Views.Gameplay.HUD;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using Resolution = Reflex.Enums.Resolution;

namespace Gameplay.Installers
{
    public class MainSceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private HUDWindow _hudWindow;
        [SerializeField] private WindowManager _windowManager;
        [SerializeField] private BranchManager _branchManager;
        [SerializeField] private Volume _postProcessVolume;
        [SerializeField] private InventoryManager _inventoryManager;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(_hudWindow);
            builder.RegisterValue(_windowManager);
            builder.RegisterValue(_branchManager);
            builder.RegisterValue(_postProcessVolume);
            builder.RegisterValue(_inventoryManager);
            builder.RegisterType(typeof(SyncEnergyManager), Lifetime.Singleton, Resolution.Eager);
            builder.RegisterType(typeof(SaveManager), Lifetime.Singleton, Resolution.Eager);
        }
    }
}