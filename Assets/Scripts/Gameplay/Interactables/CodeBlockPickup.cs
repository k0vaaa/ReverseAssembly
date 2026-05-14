using Core.Events;
using Gameplay.Events;
using UnityEngine;

namespace Gameplay.Interactables
{
    public class CodeBlockPickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventBus.Raise(new CodeBlockCollectedEvent()); 
                Destroy(gameObject);
            }
        }
    }
}
