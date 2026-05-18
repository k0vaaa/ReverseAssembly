using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class PlayerHUDView : View
    {
        [SerializeField] private TextMeshProUGUI _blocksText;
        
        [Header("Images Resources")]

        [SerializeField] private Material _mainMaterial;
        [SerializeField] private Material _corruptMaterial;
        [SerializeField] private Sprite _mainStablitityImage;
        [SerializeField] private Sprite _corruptStablitityImage;
        [SerializeField] private Sprite _corruptCrosshair;
        [SerializeField] private Sprite _mainCrosshair;
        [SerializeField] private Sprite _scannerAbilityMain;
        [SerializeField] private Sprite _scannerAbilityCorrupt;
        [SerializeField] private TextMeshProUGUI[] _texts;
        
        [Header("Images References")]
        [SerializeField] private Image _stabilityImage;
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private Image _scannerImage;
        public void UpdateBlocksCount(int count)
        {
            if (_blocksText != null)
            {
                _blocksText.text = $"{count}";
            }
        }

        public void ApplyTheme(bool isCorrupt)
        {
            Material targetMat = isCorrupt ? _corruptMaterial : _mainMaterial;
            foreach (var text in _texts)
            {
                if (text != null) 
                {
                    text.fontSharedMaterial = targetMat; 
                }
            }
            if (_stabilityImage != null)
                _stabilityImage.sprite = isCorrupt ? _corruptStablitityImage : _mainStablitityImage;

            if (_crosshairImage != null)
                _crosshairImage.sprite = isCorrupt ? _corruptCrosshair : _mainCrosshair;

            if (_scannerImage != null)
                _scannerImage.sprite = isCorrupt ? _scannerAbilityCorrupt : _scannerAbilityMain;
        }
    }
}