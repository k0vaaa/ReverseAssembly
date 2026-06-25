using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay.Enemies
{
    public class EnemyRegistry
    {
        private readonly List<AIController> _characters = new();
        private int _aliveCount;

        public event Action OnAllEnemiesDead;
        
        public IReadOnlyList<AIController> Characters => _characters;
        public int AliveCount => _aliveCount;

        public void Register(AIController enemy)
        {
            if (!_characters.Contains(enemy))
            {
                _characters.Add(enemy);
                _aliveCount++;
                enemy.StabilitySystem.OnDeath.AddListener(() => OnEnemyDeath(enemy));
            }
        }

        public void Unregister(AIController enemy)
        {
            if (_characters.Contains(enemy))
            {
                _characters.Remove(enemy);
            }
        }

        public void ClearMissing()
        {
            _characters.RemoveAll(c => c == null);
        }

        public void SetAliveCount(int count)
        {
            _aliveCount = count;
        }

        private void OnEnemyDeath(AIController enemy)
        {
            if (--_aliveCount <= 0)
            {
                OnAllEnemiesDead?.Invoke();
            }
        }
    }
}
