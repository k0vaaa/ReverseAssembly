using Core.Events;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class CodeBlockPickup : MonoBehaviour
    {
        [SerializeField] private float _givenEnergy = 30f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventBus.Raise(new CodeBlockCollectedEvent
                {
                    Energy = _givenEnergy
                }); 
                Destroy(gameObject);
            }
        }
    }
}
