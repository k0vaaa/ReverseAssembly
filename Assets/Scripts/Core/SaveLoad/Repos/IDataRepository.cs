namespace Core.SaveLoad.Repos
{
    public interface IDataRepository
    {
        public void Save<T>(string key, T data);
        public T Load<T>(string key, T defaultValue = default);
        bool HasKey(string key);
        void Delete(string key);
    }
}