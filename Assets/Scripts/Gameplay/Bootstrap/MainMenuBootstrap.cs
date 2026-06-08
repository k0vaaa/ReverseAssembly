using Core.Events;
using Core.Extensions;
using Core.Input;
using Core.SaveLoad.Interactors;
using Core.SaveLoad.PlayerSaves;
using Core.SaveLoad.Saveables;
using Core.Scenes;
using Gameplay.Events;
using Gameplay.UI.Views.MainMenu;
using Gameplay.UI.Windows;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Bootstrap
{
    public class MainMenuBootstrap : BootstrapComponent
    {
        [Inject] private MenuWindow _menuWindow;
        [Inject] private SettingsInteractor _settingsInteractor;
        [Inject] private PlayerDataInteractor _playerDataInteractor;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private InputManager _input;
       

        protected override void OnBoot()
        {
            EventBus.Subscribe<SceneLoadEvent>(e => UnbindInput()).AddTo(gameObject);
            _input.Init();
            BindInput();
            BindMenuButtons();
            
            _menuWindow.SetStackRoot<MainMenuView>();
            //InitializeSettings();
            //BindMenuButtons();
        }

        private void BindInput()
        {
            _input.OnEscapePressed += _menuWindow.Back;
        }

        private void UnbindInput()
        {
            _input.OnEscapePressed -= _menuWindow.Back;
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
            //var settingsView = _menuWindow.GetView<SettingsView>();
            var loadView = _menuWindow.GetView<LoadGameView>();

            mainMenuView.SetResumeAction(LoadGameScene);
            // Кнопки главного меню
            /*mainMenuView.SetNewGameAction(() =>
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

            mainMenuView.SetSettingsAction(() => _menuWindow.Next<SettingsView>());*/
        
            mainMenuView.SetLoadAction(() =>
            {
                _menuWindow.Next<LoadGameView>();
                loadView.ShowLoadGameMenu(_playerDataInteractor.GetAllSaves(), timestamp =>
                {
                    _playerDataInteractor.LoadByTimestamp(timestamp);
                    _ = _sceneLoader.LoadScene(SceneConstants.GameScene);
                }, timestamp =>
                {
                   _playerDataInteractor.DeleteFile(timestamp);
                });
            });

            // Кнопка возврата из меню загрузки
            loadView.SetBackButtonListener(() => _menuWindow.Back());
        }
    }
}