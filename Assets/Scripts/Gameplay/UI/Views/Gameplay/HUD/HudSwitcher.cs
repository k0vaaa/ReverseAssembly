namespace Gameplay.UI.Views.Gameplay.HUD
{
    public class HudSwitcher
    {
        public static HudSwitcher Instance { get; private set; }
        
        private readonly PlayerHUDView _hudView; 
        private bool _isCorrupt;

        public HudSwitcher(PlayerHUDView hudView)
        {
            _hudView = hudView;
            Instance = this;
        }

        public void SetCorruptMode(bool isCorrupt)
        {
            _hudView.ApplyTheme(isCorrupt);
        }
        public void ToggleTheme()
        {
            _isCorrupt = !_isCorrupt;
            _hudView.ApplyTheme(_isCorrupt);
        }
    }
}