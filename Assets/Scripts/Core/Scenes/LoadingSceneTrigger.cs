using Core.Bootstrap;

using Reflex.Attributes;
using UnityEngine;

namespace Core.Scenes
{
    public class LoadingSceneTrigger : MonoBehaviour, IInitializable
    {
        [Inject] private SceneLoader _sceneLoader;

        public void Init()
        {
            
        }
    }
}