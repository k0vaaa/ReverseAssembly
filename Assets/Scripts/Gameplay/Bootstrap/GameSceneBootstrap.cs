using Core.Events;
using Core.Extensions;
using Core.Scenes;
using Gameplay.Events;
using Gameplay.UI.Views.Gameplay.HUD;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class GameSceneBootstrap : MonoBehaviour, IBootstrapComponent
    {
        [Inject] private HUDWindow _hudWindow;
        [Inject] private SceneLoader _sceneLoader;
        public void Boot()
        {
            _hudWindow.GetView<EndGameView>().MenuClicked += () => _ = _sceneLoader.LoadScene(SceneConstants.MainMenu);
            EventBus.Subscribe<GameEndedEvent>(e =>
            {
                _hudWindow.ShowOnly<EndGameView>();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }).AddTo(gameObject);
        }
    }
}