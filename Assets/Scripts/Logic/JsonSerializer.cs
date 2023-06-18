using System.IO;
using Game.Data;
using UnityEngine;

namespace Game.Logic
{
    public class JsonSerializer
    {
        private readonly string _path;

        public JsonSerializer()
        {
            _path = Path.Combine(Application.persistentDataPath, "saveData.json");
        }

        public void SaveDataToJson(PlayerData playerData, Business[] businesses)
        {
            SaveData saveData = new SaveData();
            saveData.PlayerData = playerData;
            saveData.Businesses = businesses;

            string jsonData = JsonUtility.ToJson(saveData);
            File.WriteAllText(_path, jsonData);
        }

        public T LoadDataFromJson<T>() where T : class
        {
            if (File.Exists(_path))
            {
                string jsonData = File.ReadAllText(_path);
                return JsonUtility.FromJson<T>(jsonData);
            }

            return null;
        }
    }
}