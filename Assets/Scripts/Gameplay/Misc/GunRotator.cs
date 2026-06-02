using System;
using UnityEngine;

namespace Gameplay.Misc
{
    public class GunRotator : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _gun;
        [SerializeField] private float _smoothFactor = 1;
        private int _layerMask;
        private RaycastHit _hitInfo;
        private Vector3 _currentLookAt;

        private void Awake()
        {
            _layerMask = ~gameObject.layer;
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(_camera.position, _camera.forward, out _hitInfo, 1000f, _layerMask))
            {
                _gun.LookAt(_hitInfo.point);
            }
            /*if (_currentLookAt != _hitInfo.point)
            {
                _currentLookAt = Vector3.Lerp(_currentLookAt, _hitInfo.point,
                    Time.fixedDeltaTime * _smoothFactor);
            }*/

            
        }
    }
}