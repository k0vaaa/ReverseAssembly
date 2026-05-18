
using Core.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.Interactables;
using Reflex.Attributes;

namespace Gameplay.UI
{
    public class TerminalPuzzleView : PuzzleViewBase
    {   
        [Inject] private InventoryManager _inventoryManager;
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _blocksCountText;
        [SerializeField] private Button _repairButton;
        [SerializeField] private Button _closeButton;

        // Внимание: Здесь нам нужно откуда-то знать, сколько блоков собрано.
        // Используем InventoryManager (через синглтон или DI)
        [SerializeField] private int _requiredBlocks = 3; 

        public override void ShowPuzzle(IBuggable target)
        {
            base.ShowPuzzle(target);
            
            
            // Получаем данные о блоках (можно передавать через DI или статик)
            int currentBlocks = _inventoryManager._codeBlocks; 
            
            _blocksCountText.text = $"{currentBlocks}";
            
            _repairButton.interactable = currentBlocks >= _requiredBlocks;
            _repairButton.onClick.RemoveAllListeners();
            _repairButton.onClick.AddListener(OnWin);

            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(ClosePuzzle);
        }
    }
}