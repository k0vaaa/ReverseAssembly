using Core.Events;
using Core.Extensions;
using Core.Scenes;
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
        [Inject] private SceneLoader _sceneLoader;
        public void Boot()
        {
            _window.GetView<EndGameView>().MenuClicked += () => _ = _sceneLoader.LoadScene(SceneConstants.MainMenu);
            EventBus.Subscribe<GameEndedEvent>(e =>
            {
                _window.ShowOnly<EndGameView>();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }).AddTo(gameObject);
        }
    }
}