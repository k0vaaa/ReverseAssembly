using Core.Bootstrap;
using Core.DI;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Saveables;
using Core.Scenes;
using Core.UI;
using Gameplay.UI.Views.MainMenu;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class MainMenuBootstrap : MonoBehaviour, IInjectable, IInitializable
    {
        [Inject] private ViewManager _viewManager;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SceneLoader _sceneLoader;

        public void Init()
        {
            InitializeSettings();
            BindMenuButtons();
        }

        private void InitializeSettings()
        {
            if (!_settingsInteractor.HasSettings())
            {
                _settingsInteractor.SaveSettings(new GameSettings());
            }
        }

        private void BindMenuButtons()
        {
            var mainMenuView = _viewManager.GetView<MainMenuView>();
            var settingsView = _viewManager.GetView<SettingsView>();
            var loadView = _viewManager.GetView<LoadGameView>();

            // Кнопки главного меню
            mainMenuView.SetNewGameAction(() =>
            {
                _playerDataInteractor.StartNewGame();
                _sceneLoader.LoadScene("GameScene"); // Укажите имя вашей игровой сцены
            });

            mainMenuView.SetResumeAction(() =>
            {
                if (_playerDataInteractor.HasPlayerData())
                {
                    _playerDataInteractor.LoadLatestPlayerData();
                    _sceneLoader.LoadScene("GameScene");
                }
                else
                {
                    _playerDataInteractor.StartNewGame();
                    _sceneLoader.LoadScene("GameScene");
                }
            });

            mainMenuView.SetSettingsAction(() => _viewManager.SwitchViews(mainMenuView, settingsView));
        
            mainMenuView.SetLoadAction(() =>
            {
                _viewManager.SwitchViews(mainMenuView, loadView);
                loadView.ShowLoadGameMenu(_playerDataInteractor.GetAllSaves(), (timestamp) =>
                {
                    _playerDataInteractor.LoadByTimestamp(timestamp);
                    _sceneLoader.LoadScene("GameScene");
                });
            });

            // Кнопка возврата из меню загрузки
            loadView.SetBackButtonListener(() => _viewManager.SwitchViews(loadView, mainMenuView));
        }
    }
}