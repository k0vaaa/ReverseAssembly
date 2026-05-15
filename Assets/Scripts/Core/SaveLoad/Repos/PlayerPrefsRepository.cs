using UnityEngine;

namespace Core.SaveLoad.Repos
{
    public class PlayerPrefsRepository : IDataRepository
    {
        public void Save<T>(string key, T data)
        {
            switch (data)
            {
                case int valueInt:
                    PlayerPrefs.SetInt(key,valueInt);
                    break;
                case float valueFloat:
                    PlayerPrefs.SetFloat(key,valueFloat);
                    break;
                case string valueString:
                    PlayerPrefs.SetString(key,valueString);
                    break;
                case bool valueBool:
                    PlayerPrefs.SetInt(key, valueBool ? 1 : 0);
                    break;
                default:
                {
                    string json = JsonUtility.ToJson(data);
                    PlayerPrefs.SetString(key, json);
                    break;
                }
            }

            PlayerPrefs.Save();
        }

        public T Load<T>(string key, T defaultValue = default)
        {
            if (!PlayerPrefs.HasKey(key))
                return default;
            if (typeof(T) == typeof(float))
                return (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
            if (typeof(T) == typeof(int))
                return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
            if (typeof(T) == typeof(string))
                return (T)(object)PlayerPrefs.GetString(key, (string)(object)defaultValue);
            if (typeof(T) == typeof(bool))
                return (T)(object)(PlayerPrefs.GetInt(key, (bool)(object)defaultValue ? 1 : 0) == 1);
            string json = PlayerPrefs.GetString(key, "");
            return string.IsNullOrEmpty(json) ? defaultValue : JsonUtility.FromJson<T>(json);
        }
        

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }
}