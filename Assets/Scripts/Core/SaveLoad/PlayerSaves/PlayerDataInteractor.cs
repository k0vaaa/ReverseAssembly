using System.Collections.Generic;
using Core.SaveLoad.Repos;
using Core.SaveLoad.Saveables;

namespace Core.SaveLoad.PlayerSaves
{
    public class PlayerDataInteractor
    {
        private const string PlayerDataKey = "playerData";
        private readonly IPlayerDataRepository _playerDataRepository;
        public PlayerData CurrentSave { get; private set; }

        public PlayerDataInteractor(IPlayerDataRepository playerDataRepository)
        {
            _playerDataRepository = playerDataRepository;
        }
        
        public void StartNewGame()
        {
            CurrentSave = new PlayerData
            {
                Health = 100f,
                Position = default,
                Enemies = new List<EnemyData>()
            };
            _playerDataRepository.Save(PlayerDataKey, CurrentSave);
        }
        
        public void SavePlayerData(PlayerData playerData)
        {
            CurrentSave = playerData;
            _playerDataRepository.Save(PlayerDataKey, CurrentSave);
        }
        
        public PlayerData LoadLatestPlayerData()
        {
            CurrentSave = _playerDataRepository.Load(PlayerDataKey, new PlayerData { Enemies = new List<EnemyData>() });
            return CurrentSave;
        }

        public PlayerData LoadByTimestamp(string timestamp)
        {
            CurrentSave = _playerDataRepository.Load(PlayerDataKey, timestamp, new PlayerData { Enemies = new List<EnemyData>() });
            return CurrentSave;
        }

        public List<string> GetAllSaves()
        {
            return _playerDataRepository.GetAllTimestamps();
        }

        public bool HasPlayerData()
        {
            return _playerDataRepository.HasKey(PlayerDataKey);
        }

        public void DeletePlayerData()
        {
            _playerDataRepository.Delete(PlayerDataKey);
        }

        public void DeleteFile(string timestamp)
        {
            _playerDataRepository.DeleteByTimestamp(timestamp, PlayerDataKey);
        }
    }
}