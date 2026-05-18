using Gameplay.UI;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class BuggableBase : MonoBehaviour, IBuggable
    {
        [SerializeField] protected PuzzleViewBase _puzzleView;
        [SerializeField] protected Gameplay.Events.WorldBranch _requiredBranch = Gameplay.Events.WorldBranch.Alpha;

        public bool IsBugged { get; protected set; } = true;
        public virtual bool IsInteractableInCurrentBranch(Gameplay.Events.WorldBranch branch)
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
    }
}