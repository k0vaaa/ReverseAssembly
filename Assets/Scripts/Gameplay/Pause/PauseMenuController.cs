using Core.Bootstrap;

using Core.Events;
using Core.Extensions;
using Core.Pause;
using Core.SaveLoad.PlayerSaves;
using Core.Scenes;
using Gameplay.UI.Views.Gameplay;
using Gameplay.UI.Views.Gameplay.HUD;
using Gameplay.UI.Views.MainMenu;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Pause
{
    public class PauseMenuController : MonoBehaviour, IInitializable
    {
        [Inject] private HUDWindow _hudWindow;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PlayerDataInteractor _playerDataInteractor;
    
        private PauseView _pauseView;
        private LoadGameView _loadGameView;

        public void Init()
        {
            EventBus.Subscribe<GamePauseEvent>(OnPauseStateChanged).AddTo(gameObject);
        
            _pauseView = _hudWindow.GetView<PauseView>();
            _loadGameView = _hudWindow.GetView<LoadGameView>();

            // Привязка кнопок
            /*_pauseView.SetMainMenuButtonListener(() => 
            {
                PauseManager.SetPause(false); // Снимаем паузу перед выходом
                _sceneLoader.LoadScene("MainMenu");
            });

            _pauseView.SetLoadLastButtonListener(() => 
            {
                PauseManager.SetPause(false);
                _playerDataInteractor.LoadLatestPlayerData();
                _sceneLoader.LoadScene("LoadingScene"); // Загружаем текущую сцену заново
            });

            _pauseView.SetToLoadChooseButtonListener(() => 
            {
                _window.SwitchViews(_pauseView, _loadGameView);
                _loadGameView.ShowLoadGameMenu(_playerDataInteractor.GetAllSaves(), LoadSelected);
            });*/

            _loadGameView.SetBackButtonListener(() => _hudWindow.SwitchViews(_loadGameView, _pauseView));
        }

        private void LoadSelected(string timestamp)
        {
            PauseManager.SetPause(false);
            _playerDataInteractor.LoadByTimestamp(timestamp);
            _sceneLoader.LoadScene("LoadingScene");
        }

        private void OnPauseStateChanged(GamePauseEvent eventData)
        {
            if (eventData.IsPaused)
                _pauseView.Show();
            else
                _pauseView.Hide();
        }
    }
}