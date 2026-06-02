using Core.Bootstrap;

using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Saveables;
using Core.Scenes;
using Core.UI;
using Gameplay.UI.Views.MainMenu;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class MainMenuBootstrap : MonoBehaviour, IBootstrapComponent
    {
        [Inject] private Window _menuWindow;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SceneLoader _sceneLoader;
       

        public void Boot()
        {
            _menuWindow.GetView<MainMenuView>().SetResumeAction(LoadGameScene);
            //InitializeSettings();
            //BindMenuButtons();
        }

        private void LoadGameScene()
        {
            print(1);
            _ = _sceneLoader.LoadScene(SceneConstants.GameScene);
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
            var mainMenuView = _menuWindow.GetView<MainMenuView>();
            var settingsView = _menuWindow.GetView<SettingsView>();
            var loadView = _menuWindow.GetView<LoadGameView>();

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

            mainMenuView.SetSettingsAction(() => _menuWindow.SwitchViews(mainMenuView, settingsView));
        
            mainMenuView.SetLoadAction(() =>
            {
                _menuWindow.SwitchViews(mainMenuView, loadView);
                loadView.ShowLoadGameMenu(_playerDataInteractor.GetAllSaves(), (timestamp) =>
                {
                    _playerDataInteractor.LoadByTimestamp(timestamp);
                    _sceneLoader.LoadScene("GameScene");
                });
            });

            // Кнопка возврата из меню загрузки
            loadView.SetBackButtonListener(() => _menuWindow.SwitchViews(loadView, mainMenuView));
        }
    }
}