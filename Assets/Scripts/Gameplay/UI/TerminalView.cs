using Core.UI;
using Gameplay.Events;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class TerminalView : View
    {
        [SerializeField] private TextMeshProUGUI _branchInfoText;
        [SerializeField] private TextMeshProUGUI _promptText;

        public void UpdateInfo(WorldBranch currentBranch)
        {
            if (_branchInfoText != null)
            {
                _branchInfoText.text = $"CURRENT BRANCH: {currentBranch.ToString().ToUpper()}";
            }

            if (_promptText != null)
            {
                WorldBranch targetBranch = currentBranch == WorldBranch.Main ? WorldBranch.Alpha : WorldBranch.Main;
                _promptText.text = $"PRESS [E] TO JUMP TO {targetBranch.ToString().ToUpper()}";
            }
        }
    }
}
