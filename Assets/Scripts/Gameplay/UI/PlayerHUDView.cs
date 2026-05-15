using Core.UI;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class PlayerHUDView : View
    {
        [SerializeField] private TextMeshProUGUI _blocksText;

        public void UpdateBlocksCount(int count)
        {
            if (_blocksText != null)
            {
                _blocksText.text = $"Blocks: {count}";
            }
        }
    }
}
