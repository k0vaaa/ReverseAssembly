using Core.UI;
using Reflex.Core;
using UnityEngine;

namespace Gameplay.Installers
{
    public class MenuInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Window _window;


        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(_window);
        }
    }
}