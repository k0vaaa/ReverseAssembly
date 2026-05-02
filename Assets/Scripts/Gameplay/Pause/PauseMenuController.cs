using Core.Bootstrap;
using Core.DI;
using Core.Events;
using Core.Extensions;
using Core.Pause;
using Core.SaveLoad.PlayerSaves;
using Core.Scenes;
using Core.UI;
using Gameplay.UI.Views.Gameplay;
using Gameplay.UI.Views.MainMenu;
using UnityEngine;

namespace Gameplay.Pause
{
    public class PauseMenuController : MonoBehaviour, IInjectable, IInitializable
    {
        [Inject] private ViewManager _viewManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private PlayerDataInteractor _playerDataInteractor;
    
        private PauseView _pauseView;
        private LoadGameView _loadGameView;

        public void Init()
        {
            EventBus.Subscribe<GamePauseEvent>(OnPauseStateChanged).AddTo(gameObject);
        
            _pauseView = _viewManager.GetView<PauseView>();
            _loadGameView = _viewManager.GetView<LoadGameView>();

            // Привязка кнопок
            _pauseView.SetMainMenuButtonListener(() => 
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
                _viewManager.SwitchViews(_pauseView, _loadGameView);
                _loadGameView.ShowLoadGameMenu(_playerDataInteractor.GetAllSaves(), LoadSelected);
            });

            _loadGameView.SetBackButtonListener(() => _viewManager.SwitchViews(_loadGameView, _pauseView));
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