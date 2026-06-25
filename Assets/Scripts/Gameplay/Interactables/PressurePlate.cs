using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Interactables
{
    public class PressurePlate : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string _targetTag = "PhysicsBox"; 
        [SerializeField] private UnityEvent _onPlatePressed;     // Что делать при нажатии
        [SerializeField] private UnityEvent _onPlateReleased;    // Что делать при отпускании

        private int _objectsOnPlate = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_targetTag))
            {
                _objectsOnPlate++;
                if (_objectsOnPlate == 1) 
                {
                    _onPlatePressed?.Invoke();
                }
                Debug.Log("БЫЛО!");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_targetTag))
            {
                _objectsOnPlate--;
                if (_objectsOnPlate <= 0) 
                {
                    _objectsOnPlate = 0;
                    _onPlateReleased?.Invoke();
                }
            }
        }
    }
}