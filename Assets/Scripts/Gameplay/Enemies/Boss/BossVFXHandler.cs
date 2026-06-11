using UnityEngine;

namespace Gameplay.Enemies
{
    public class BossVFXHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _particles;
        [SerializeField] private GameObject _fist;

        private void Start()
        {
            CreateParticles();
        }

        private void CreateParticles()
        {
            if (_particles != null && _particles.Length > 0 && _fist != null)
            {
                var part = _particles[Random.Range(0, _particles.Length)];
                Instantiate(part, _fist.transform.position, Quaternion.identity, _fist.transform);
            }
        }
    }
}
