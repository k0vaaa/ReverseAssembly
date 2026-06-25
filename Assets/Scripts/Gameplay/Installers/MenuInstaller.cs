using Gameplay.UI.Windows;
using Reflex.Core;
using UnityEngine;

namespace Gameplay.Installers
{
    public class MenuInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private MenuWindow _window;


        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(_window);
        }
    }
}