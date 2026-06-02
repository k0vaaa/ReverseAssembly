using Core.Audio;
using Core.Events;
using Core.Extensions;
using Core.Input;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Repos;
using Core.UI;
using Gameplay.Events;
using Gameplay.UI;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class MainBootstrap : MonoBehaviour, IBootstrapComponent
    {
        [Inject] private Window _window;
        public void Boot()
        {
            EventBus.Subscribe<GameEndedEvent>(e =>
            {
                _window.ShowOnly<EndGameView>();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }).AddTo(gameObject);
        }
    }
}