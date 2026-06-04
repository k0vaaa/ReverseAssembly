using Core.UI;
using Gameplay.UI.Views.Gameplay.HUD;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Views.Gameplay.Terminal
{
    public class TerminalScannerView : View
    {
        [SerializeField] private SkillView _scannerSkillView;
        public SkillView ScannerSkillView => _scannerSkillView;
        [SerializeField] private TextMeshProUGUI _objectName;
        [SerializeField] private TextMeshProUGUI _objectInfo;

        public void SetNameText(string text) => _objectName.text = text;
        public void SetInfoText(string text) => _objectInfo.text = text;
    
    }
}