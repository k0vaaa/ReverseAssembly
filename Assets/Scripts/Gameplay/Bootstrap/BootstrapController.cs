using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class BootstrapController : MonoBehaviour
    {
        [SerializeField] private List<InterfaceReference<IBootstrapComponent>> _bootstraps;

        public void Boot()
        {
            foreach (var bootstrap in _bootstraps)
            {
                bootstrap.Value.Boot();
                Debug.Log($"{bootstrap.Value.GetType().Name} booted");
            }
        }
    }
}