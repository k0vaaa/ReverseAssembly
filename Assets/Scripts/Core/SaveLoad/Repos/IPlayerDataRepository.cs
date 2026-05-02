using System.Collections.Generic;

namespace Core.SaveLoad.Repos
{
    public interface IPlayerDataRepository : IDataRepository
    {
        public T Load<T>(string key, string timestamp, T defaultValue = default);
        public List<string> GetAllTimestamps();
    }
}