using DG.Tweening;
using ExternalAssets.QuickOutline.Scripts;
using Gameplay.UI;
using UnityEngine;
using static ExternalAssets.QuickOutline.Scripts.Outline;

namespace Gameplay.Interactables
{
    public class BuggableBase : MonoBehaviour, IBuggable
    {
        [SerializeField] protected PuzzleViewBase _puzzleView;
        [SerializeField] protected Events.WorldBranch _requiredBranch = Events.WorldBranch.Alpha;
        [SerializeField] protected float _scannerVisibilityTime = 2f;
        protected Outline _outline;
        protected Sequence _sequence;

        public bool IsBugged { get; protected set; } = true;
        public virtual bool IsInteractableInCurrentBranch(Events.WorldBranch branch)
        {
            return branch == _requiredBranch;
        }
        public virtual void OnScanned(bool isScanning)
        {
            
        }

        public virtual void OnInteract()
        {
            
        }

        public virtual void FixBug()
        {
            
        }

        public void Visualize()
        {
            if (_outline != null)
            {
                var mode = _outline.OutlineMode;
                _outline.OutlineMode = Mode.OutlineAll;
                _outline.enabled = true;
                _sequence?.Kill();
                _sequence = DOTween.Sequence();

                _sequence.AppendInterval(_scannerVisibilityTime)
                    .OnKill(() =>
                    {
                        _outline.enabled = false;
                        _outline.OutlineMode = mode;
                    });
                _sequence.Play();
            }
        }
    }
}