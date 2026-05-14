using UnityEngine;
using UnityEngine.UI;
using Gameplay.Interactables;

namespace Gameplay.UI
{
    public class PhysicsPuzzleView : PuzzleViewBase
    {
        [Header("Stub Debugging")]
        [SerializeField] private Button _winButtonStub;
        [SerializeField] private Button _closeButtonStub;

        public override void ShowPuzzle(IBuggable target)
        {
            base.ShowPuzzle(target);
            
            if (_winButtonStub != null)
            {
                _winButtonStub.onClick.RemoveAllListeners();
                _winButtonStub.onClick.AddListener(OnWin);
            }

            if (_closeButtonStub != null)
            {
                _closeButtonStub.onClick.RemoveAllListeners();
                _closeButtonStub.onClick.AddListener(ClosePuzzle);
            }
        }
    }
}
