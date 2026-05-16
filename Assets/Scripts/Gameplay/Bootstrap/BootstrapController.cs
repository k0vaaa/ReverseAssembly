using System;
using System.Collections.Generic;
using Core.Bootstrap;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.UI;
using Core.Utilities;
using Gameplay.UI.Views.Gameplay;
using Reflex.Attributes;
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

        public void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}