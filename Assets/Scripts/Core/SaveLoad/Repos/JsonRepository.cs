using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Core.SaveLoad.Repos
{
    public class JsonRepository : IPlayerDataRepository
    {
        private readonly string _basePath;
        private readonly string _filePrefix;

        public JsonRepository(string fileName)
        {
            _filePrefix = Path.GetFileNameWithoutExtension(fileName);
            _basePath = Path.Combine(Application.persistentDataPath, _filePrefix);
        }

        public void Save<T>(string key, T data)
        {
            // Формируем имя файла с таймстемпом (например, player_data_20250414123045.json)
            //string timestamp = DateTime.Now.ToString("dd.MM.yyyy-HH_mm_ss");
            string timestamp = "01.01.2025-00_00_00";
            string filePath = Path.Combine(Application.persistentDataPath, $"{_filePrefix}_{timestamp}.json");

            // Создаем или обновляем словарь
            var dataDict = new SerializableDictionary
            {
                [key] = data
            };

            try
            {
                // Записываем данные в новый файл
                File.WriteAllText(filePath, JsonConvert.SerializeObject(dataDict, Formatting.Indented));
                Debug.Log($"Saved data to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save to {filePath}: {ex.Message}");
            }
        }
        

        public T Load<T>(string key, T defaultValue)
        {
            return Load<T>(key, null);
        }

        public T Load<T>(string key, string timestamp)
        {
            string filePath = timestamp != null
                ? Path.Combine(Application.persistentDataPath, $"{_filePrefix}_{timestamp}.json")
                : GetLatestFile();

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Debug.Log($"No file found: {filePath}");
                return default;
            }

            try
            {
                var dataDict = JsonConvert.DeserializeObject<SerializableDictionary>(File.ReadAllText(filePath));
                if (dataDict != null && dataDict.ContainsKey(key))
                {
                    if (dataDict[key] is JObject jObject)
                    {
                        return jObject.ToObject<T>();
                    }
                }

                Debug.Log($"Key {key} not found in {filePath}");
                return default;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load from {filePath}: {ex.Message}");
                return default;
            }
        }

        public T Load<T>(string key, string timestamp, T defaultValue = default)
        {
            return Load<T>(key, timestamp);
        }

        public List<string> GetAllTimestamps()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath, $"{_filePrefix}_*.json");
            List<string> timestamps = new List<string>();

            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string timestamp = fileName.Substring(_filePrefix.Length + 1); // Убираем префикс и "_"
                timestamps.Add(timestamp);
            }

            timestamps.Sort((a, b) => b.CompareTo(a)); // Сортируем от новых к старым
            return timestamps;
        }

        public bool HasKey(string key)
        {
            string latestFile = GetLatestFile();
            if (string.IsNullOrEmpty(latestFile))
            {
                Debug.Log($"No file found: {latestFile}");
                return false;
            }

            try
            {
                var dataDict = JsonConvert.DeserializeObject<SerializableDictionary>(File.ReadAllText(latestFile));
                return dataDict != null && dataDict.ContainsKey(key);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to check key in {latestFile}: {ex.Message}");
                return false;
            }
        }

        public void Delete(string key)
        {
            string latestFile = GetLatestFile();
            if (string.IsNullOrEmpty(latestFile))
                return;

            try
            {
                var dataDict = JsonConvert.DeserializeObject<SerializableDictionary>(File.ReadAllText(latestFile));
                if (dataDict != null && dataDict.ContainsKey(key))
                {
                    dataDict.Remove(key);
                    File.WriteAllText(latestFile, JsonConvert.SerializeObject(dataDict));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to delete key from {latestFile}: {ex.Message}");
            }
        }

        private string GetLatestFile()
        {
            // Находим все файлы с префиксом _filePrefix
            string[] files = Directory.GetFiles(Application.persistentDataPath, $"{_filePrefix}_*.json");
            if (files.Length == 0)
                return null;

            // Сортируем по имени файла (таймстемп в имени обеспечивает хронологический порядок)
            Array.Sort(files);
            return files[^1]; // Возвращаем последний (самый новый) файл
        }

        [Serializable]
        private class SerializableDictionary : Dictionary<string, object>
        {
        }
    }
}