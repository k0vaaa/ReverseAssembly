using System.Collections.Generic;
using Core.Extensions;
using Core.Utilities;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class BootstrapController : MonoBehaviour
    {
        [SerializeField] private List<InterfaceReference<BootstrapComponent>> _bootstraps;
        private List<BootstrapComponent> _queue = new ();
        public void Boot()
        {
            foreach (var bootstrap in _bootstraps)
            {
                _queue.Add(bootstrap.Value);
            }

            for (var i = 0; i < _queue.Count - 1; i++)
            {
                _queue[i].OnBooted += _queue[i + 1].Boot;
            }
            _queue[0]?.Boot();
            /*foreach (var bootstrap in _bootstraps)
            {
                bootstrap.Value.Boot();
                bootstrap.Value.OnBooted += () => Debug.Log($"{bootstrap.Value.TypeName()} booted");
            }*/
        }
    }
}