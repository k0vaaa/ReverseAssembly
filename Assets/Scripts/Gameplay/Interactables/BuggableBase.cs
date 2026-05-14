using Core.UI;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class BuggableBase : MonoBehaviour, IBuggable
    {
        [SerializeField] protected PuzzleViewBase _puzzleView;
        
        public bool IsBugged { get; protected set; } = true;

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