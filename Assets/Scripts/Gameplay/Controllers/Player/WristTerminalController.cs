using System;
using Core.Bootstrap;
using Core.DI;
using Gameplay.Core;
using UnityEngine;
using InputManager = Core.Input.InputManager;

namespace Gameplay.Controllers.Player
{
    public class WristTerminalController : MonoBehaviour, IInjectable, IInitializable, IDisposable
    {
        [Inject] private InputManager _inputManager;
        [Inject] private BranchManager _branchManager;

        public void Init()
        {
            _inputManager.OnBranchTogglePressed += HandleBranchToggle;
        }

        private void HandleBranchToggle()
        {
            // Здесь в будущем можно добавить проверку: если ХП < 20% или идет анимация атаки - не переключать
            _branchManager.ToggleBranch();
            // TODO: Вызвать анимацию левой руки (щелчок по терминалу) и звук
        }

        public void Dispose()
        {
            _inputManager.OnBranchTogglePressed -= HandleBranchToggle;
        }
    }
}