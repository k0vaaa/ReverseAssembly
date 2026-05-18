using UnityEngine;

namespace Gameplay.Controllers.Player
{
    public class VFXController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _scannerEffect;

        public void PlayScanner(bool play = true)
        {
            if (play)
            {
                _scannerEffect.Play();
            }
            else
            {
                _scannerEffect.Stop();
            }
        }
    }
}