using Gameplay.Combat.Health;
using Gameplay.UI.Views.Gameplay.HUD;
using UnityEngine;

namespace Gameplay.Enemies
{
    public class StabilityUIBinder : MonoBehaviour
    {
        [SerializeField] private StabilitySystem _stabilitySystem;
        [SerializeField] private StabilityBarView _view;

        private void Start()
        {
            _stabilitySystem.onStabilityChanged.AddListener(_view.ChangeValue);
            _view.ChangeValue(_stabilitySystem.Stability, _stabilitySystem.MaxStability);
        }

        private void OnDestroy()
        {
            _stabilitySystem.onStabilityChanged.RemoveListener(_view.ChangeValue);
        }
    }
}