using Core.Inventory;
using Core.UI;
using Gameplay.Core;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using Resolution = Reflex.Enums.Resolution;

namespace Core.Installers
{
    public class MainSceneInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private ViewManager _viewManager;
        [SerializeField] private BranchManager _branchManager;
        [SerializeField] private Volume _postProcessVolume;
        private SyncEnergyManager _syncEnergyManager;
        private AnimationClip _clip;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(_viewManager);
            builder.RegisterValue(_branchManager);
            builder.RegisterValue(_postProcessVolume);
            builder.RegisterType(typeof(SyncEnergyManager), Lifetime.Singleton, Resolution.Eager);
        }
    }
}