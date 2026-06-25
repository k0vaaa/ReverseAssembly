using Gameplay.Bootstrap;
using UnityEngine;

namespace Gameplay.Test
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private BootstrapController _controller;

        private void Awake()
        {
            _controller.Boot();
        }
    }
}