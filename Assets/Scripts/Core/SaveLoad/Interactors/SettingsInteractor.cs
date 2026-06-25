using Core.SaveLoad.Repos;
using Core.SaveLoad.Saveables;

namespace Core.SaveLoad.Interactors
{
    public class SettingsInteractor
    {
        private const string SettingsKey = "gameSettings";
        private readonly IDataRepository _playerPrefsRepository;

        public SettingsInteractor(IDataRepository playerPrefsRepository)
        {
            _playerPrefsRepository = playerPrefsRepository;
        }

        public void SaveSettings(GameSettings settings)
        {
            _playerPrefsRepository.Save(SettingsKey, settings);
        }

        public GameSettings LoadSettings()
        {
            //todo Прикрутить настройки
            //return _playerPrefsRepository.Load(SettingsKey,new GameSettings());
            return new GameSettings
            {
                EnemiesPower = 1,
                PeaceMode = false
            };
        }

        public bool HasSettings()
        {
            return _playerPrefsRepository.HasKey(SettingsKey);
        }

        public void DeleteSettings()
        {
            _playerPrefsRepository.Delete(SettingsKey);
        }
        
    }
}