using Gameplay.Interactables;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Views.Gameplay.Terminal
{
    public class SliderPuzzleView : PuzzleViewBase
    {
        [Header("UI Elements")]
        [SerializeField] private Slider[] _sliders;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _closeButton;

        [Header("Settings")]
        [SerializeField] private float _targetMin = 0.45f;
        [SerializeField] private float _targetMax = 0.55f;

        public override void ShowPuzzle(IBuggable target)
        {
            base.ShowPuzzle(target);
            
            // Randomize sliders and subscribe
            foreach (var slider in _sliders)
            {
                if (slider == null) continue;
                slider.value = Random.Range(0f, 1f);
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener(CheckWinCondition);
            }
            
            if (_applyButton != null)
            {
                _applyButton.interactable = false;
                _applyButton.onClick.RemoveAllListeners();
                _applyButton.onClick.AddListener(OnWin);
            }

            if (_closeButton != null)
            {
                _closeButton.onClick.RemoveAllListeners();
                _closeButton.onClick.AddListener(ClosePuzzle);
            }

            CheckWinCondition(0f);
        }

        private void CheckWinCondition(float _)
        {
            if (_sliders == null || _sliders.Length == 0) return;

            bool isWin = true;
            foreach (var slider in _sliders)
            {
                if (slider == null) continue;
                if (slider.value < _targetMin || slider.value > _targetMax)
                {
                    isWin = false;
                    break;
                }
            }
            
            if (_applyButton != null)
            {
                _applyButton.interactable = isWin;
            }
            
            // Опционально: здесь можно добавить проигрывание звука сдвига ползунка
        }

        public override void ClosePuzzle()
        {
            base.ClosePuzzle();
            if (_sliders != null)
            {
                foreach (var slider in _sliders)
                {
                    if (slider != null) slider.onValueChanged.RemoveAllListeners();
                }
            }
        }
    }
}
