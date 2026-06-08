namespace Gameplay.Enemies
{
    public class BossDirector
    {
        private readonly EnemyRegistry _registry;
        private readonly EnemySpawner _spawner;
        private bool _bossSpawned;

        public BossDirector(EnemyRegistry registry, EnemySpawner spawner)
        {
            _registry = registry;
            _spawner = spawner;
            
            _registry.OnAllEnemiesDead += HandleAllEnemiesDead;
        }

        public void MarkBossSpawned()
        {
            _bossSpawned = true;
        }

        private void HandleAllEnemiesDead()
        {
            if (!_bossSpawned)
            {
                _bossSpawned = true;
                _spawner.SpawnBoss();
            }
        }
        
        ~BossDirector()
        {
            _registry.OnAllEnemiesDead -= HandleAllEnemiesDead;
        }
    }
}
