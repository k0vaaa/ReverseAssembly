using UnityEngine;

namespace Gameplay.Enemies.VFX
{
    public class EnemyVFXController : MonoBehaviour
    {
        [SerializeField] private GlitchVFX _glitch;

        public void PlayGlitch()
        {
            _glitch.DoGlitch();
        }
    }
}