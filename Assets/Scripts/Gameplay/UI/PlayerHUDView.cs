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
        // [SerializeField] private Sprite _branchAbilityMain;
        // [SerializeField] private Sprite _branchAbilityCorrupt;
        [SerializeField] private Sprite _gunAbilityMain;
        [SerializeField] private Sprite _gunAbilityCorrupt;
        [SerializeField] private Sprite _meleeAbilityMain;
        [SerializeField] private Sprite _meleeAbilityCorrupt;
        [SerializeField] private Sprite _mainCodeblocks;
        [SerializeField] private Sprite _corruptCodeblocks;

        [SerializeField] private Sprite _corruptEnegry;
        [SerializeField] private Sprite _mainEnegry;
        [SerializeField] private TextMeshProUGUI[] _texts;
        
        [Header("Images References")]
        [SerializeField] private Image _stabilityImage;
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private Image _scannerImage;
        [SerializeField] private Image _branchImage;
        [SerializeField] private Image _gunImage;
        [SerializeField] private Image _meleeImage;
        [SerializeField] private Image _codeblocksImage;
        [SerializeField] private Image _energyImage;

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
            // if (_branchImage != null)
            //     _branchImage.sprite = isCorrupt ? _branchAbilityCorrupt : _branchAbilityMain;
            
            if (_gunImage != null)
                _gunImage.sprite = isCorrupt ? _gunAbilityCorrupt : _gunAbilityMain;
            if (_meleeImage != null)
                _meleeImage.sprite = isCorrupt ? _meleeAbilityCorrupt : _meleeAbilityMain;
            
            if (_codeblocksImage != null)
                _codeblocksImage.sprite = isCorrupt ? _corruptCodeblocks : _mainCodeblocks;
            if (_energyImage != null)
                _energyImage.sprite = isCorrupt ? _corruptEnegry : _mainEnegry;
        }
    }
}