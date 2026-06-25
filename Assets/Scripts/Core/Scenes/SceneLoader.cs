using Core.Events;
using Core.Pause;
using Gameplay.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Scenes
{
    public class SceneLoader
    {
        private Scene _currentScene = SceneManager.GetActiveScene();
        
        

        public async Awaitable LoadScene(string nextScene)
        {
            PauseManager.SetPause(true);
            EventBus.Raise(new SceneLoadEvent());
            var loadingScene = SceneManager.LoadSceneAsync(SceneConstants.LoadingScene, LoadSceneMode.Additive);
            await loadingScene;
            await SceneManager.UnloadSceneAsync(_currentScene);
            await SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
            _currentScene = SceneManager.GetSceneByName(nextScene);
            SceneManager.SetActiveScene(_currentScene);
            SceneManager.UnloadSceneAsync(SceneConstants.LoadingScene);
            PauseManager.SetPause(false);
        }
        
    }

    public static class SceneConstants
    {
        public static string LoadingScene = "LoadingScene";
        public static string GameScene = "ProjectAssembly123";
        public static string MainMenu = "MainMenu";
    
    }
}