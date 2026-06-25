using Gameplay.Combat.Health;
using UnityEngine;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(StabilitySystem))]
    public class EnemyLootSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _codeBlockPrefab;
        private StabilitySystem _stabilitySystem;

        private void Awake()
        {
            _stabilitySystem = GetComponent<StabilitySystem>();
        }

        private void OnEnable()
        {
            _stabilitySystem.OnDeath.AddListener(SpawnLoot);
        }

        private void OnDisable()
        {
            _stabilitySystem.OnDeath.RemoveListener(SpawnLoot);
        }

        private void SpawnLoot()
        {
            if (_codeBlockPrefab != null)
            {
                Instantiate(_codeBlockPrefab, transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }
}
