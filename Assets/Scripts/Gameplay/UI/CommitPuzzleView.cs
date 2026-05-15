using UnityEngine;
using Gameplay.Interactables;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class CommitPuzzleView : PuzzleViewBase
    {
        [Header("UI Elements")]
        [Tooltip("Список всех перетаскиваемых строк кода")]
        [SerializeField] private DraggableCodeLine[] _codeLines;
        
        [Tooltip("Установите сюда Transforms строк в правильном порядке (сверху вниз)")]
        [SerializeField] private Transform[] _correctOrder;

        [SerializeField] private Button _closeButton;
        public override void ShowPuzzle(IBuggable target)
        {
            base.ShowPuzzle(target);
            
            if (_codeLines == null || _codeLines.Length == 0) return;

            // Перемешиваем строки
            for (int i = 0; i < 15; i++)
            {
                int rnd = Random.Range(0, _codeLines.Length);
                _codeLines[rnd].transform.SetAsLastSibling();
            }

            // Подписываемся на события конца перетаскивания
            foreach (var line in _codeLines)
            {
                if (line == null) continue;
                line.OnDragEnded -= CheckWinCondition;
                line.OnDragEnded += CheckWinCondition;
            }
            
            if (_closeButton != null)
            {
                _closeButton.onClick.RemoveAllListeners();
                _closeButton.onClick.AddListener(ClosePuzzle);
            }
        }

        private void CheckWinCondition()
        {
            if (_correctOrder == null || _correctOrder.Length == 0) return;

            for (int i = 0; i < _correctOrder.Length - 1; i++)
            {
                // Проверяем относительный порядок: каждый следующий элемент должен быть НИЖЕ (иметь SiblingIndex больше)
                if (_correctOrder[i] != null && _correctOrder[i+1] != null)
                {
                    if (_correctOrder[i].GetSiblingIndex() > _correctOrder[i+1].GetSiblingIndex())
                    {
                        return; // Порядок еще не верный
                    }
                }
            }
            
            // Если мы дошли сюда, значит порядок верный! ПАЗЛ РЕШЕН
            foreach (var line in _codeLines)
            {
                if (line != null) line.OnDragEnded -= CheckWinCondition;
            }
            
            OnWin();
        }
        
        public override void ClosePuzzle()
        {
            base.ClosePuzzle();
            if (_codeLines != null)
            {
                foreach (var line in _codeLines)
                {
                    if (line != null) line.OnDragEnded -= CheckWinCondition;
                }
            }
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}
