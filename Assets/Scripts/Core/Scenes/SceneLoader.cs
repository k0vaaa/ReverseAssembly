using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Scenes
{
    public class SceneLoader
    {
        public static string NextSceneName;

        // Метод для вызова из других скриптов
        public void LoadScene(string sceneName)
        {
            NextSceneName = sceneName;
            SceneManager.LoadScene("LoadingScene");
        }

        // Этот метод будет вызываться самим LoadingSceneManager на сцене загрузки
        public async void LoadNextSceneAsync()
        {
            if (string.IsNullOrEmpty(NextSceneName)) return;
        
            AsyncOperation operation = SceneManager.LoadSceneAsync(NextSceneName);
            while (!operation.isDone)
            {
                await System.Threading.Tasks.Task.Yield();
            }
        }
    }
}