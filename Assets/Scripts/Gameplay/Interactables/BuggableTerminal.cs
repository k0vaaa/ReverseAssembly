
using Core.Input;
using Core.UI;
using Gameplay.UI;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class BuggableTerminal: BuggableBase
    {
        [Inject] private ViewManager _viewManager;
        [Inject] private InputManager _inputManager;
        [SerializeField] private GameObject bridgeObj;
        
        
        private void Awake()
        {
            // Изначально мост скрыт
            if (bridgeObj != null) bridgeObj.SetActive(false);
            
        }
        
        public override void OnInteract()
        {
            // Ищем наш новый PuzzleView
            var puzzle = _puzzleView as TerminalPuzzleView; 
            puzzle.ShowPuzzle(this);
        
            // Отключаем управление игроком (как в BuggablePhysicsBox)
            _inputManager.DisablePlayerInput();
        }

        public override void FixBug()
        {
            Debug.Log("Терминал починен!");
            IsBugged = false;
            if (bridgeObj != null) bridgeObj.SetActive(true);
        }
    }
}