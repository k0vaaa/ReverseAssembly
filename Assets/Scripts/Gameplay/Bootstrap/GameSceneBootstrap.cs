using Core.Scenes;
using Gameplay.UI.Views.Gameplay.HUD;
using Reflex.Attributes;

namespace Gameplay.Bootstrap
{
    public class GameSceneBootstrap : BootstrapComponent
    {
        [Inject] private HUDWindow _hudWindow;
        [Inject] private SceneLoader _sceneLoader;
        public void Boot()
        {
            _hudWindow.GetView<EndGameView>().MenuClicked += () => _ = _sceneLoader.LoadScene(SceneConstants.MainMenu);
        }
    }
}