using Core.Bootstrap;
using Core.DI;
using UnityEngine;

namespace Core.Scenes
{
    public class LoadingSceneTrigger : MonoBehaviour, IInjectable, IInitializable
    {
        [Inject] private SceneLoader _sceneLoader;

        public void Init()
        {
            _sceneLoader.LoadNextSceneAsync();
        }
    }
}