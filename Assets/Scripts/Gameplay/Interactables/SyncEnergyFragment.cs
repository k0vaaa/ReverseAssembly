using Core.Audio;

using Gameplay.Core;
using Reflex.Attributes;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class SyncEnergyFragment : MonoBehaviour
    {
        [Inject] private SyncEnergyManager _syncEnergyManager;
        [Inject] private AudioManager _audioManager;

        [SerializeField] private AudioClip _pickupSound;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _syncEnergyManager.AddEnergy(15f);
                
                if (_pickupSound != null)
                {
                    _ = _audioManager.PlaySFX(_pickupSound, transform.position, 1f);
                }
                
                Destroy(gameObject);
            }
        }
    }
}
