using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class EndGameView : View
    {
        [SerializeField] private Button _quitButton;

        private void Awake()
        {
            _quitButton.onClick.AddListener(Application.Quit);
        }
    }
}