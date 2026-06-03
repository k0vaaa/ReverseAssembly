using Gameplay.Combat.Health;
using Gameplay.UI.Views.Gameplay;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class StabilityUIBinder : MonoBehaviour
    {
        [SerializeField] private StabilitySystem _stabilitySystem;
        [SerializeField] private StabilityBarView _view;

        private void Awake()
        {
            _stabilitySystem.onStabilityChanged.AddListener(_view.ChangeValue);
        }

        private void OnDestroy()
        {
            _stabilitySystem.onStabilityChanged.RemoveListener(_view.ChangeValue);
        }
    }
}