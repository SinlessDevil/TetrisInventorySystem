using System;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.PersistenceProgress.Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Code.Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PlayerDataKey = "PlayerData";
        
        private readonly IPersistenceProgressService _progressService;

        public SaveLoadService(IPersistenceProgressService progressService)
        {
            _progressService = progressService;
        }

        [Button]
        public void SaveProgress() => Save(_progressService.PlayerData);

        [Button]
        public void Save(PlayerData playerData)
        {
            byte[] serializedValue = SerializationUtility.SerializeValue(playerData, DataFormat.JSON);
            string base64String = Convert.ToBase64String(serializedValue);
            PlayerPrefs.SetString(key: PlayerDataKey, base64String);
            PlayerPrefs.Save();
        }

        [Button]
        public PlayerData Load()
        {
            string base64String = PlayerPrefs.GetString(key: PlayerDataKey, string.Empty);

            if (base64String == string.Empty)
                return null;

            byte[] serializedValue = Convert.FromBase64String(base64String);
            return SerializationUtility.DeserializeValue<PlayerData>(serializedValue, DataFormat.JSON);
        }
    }
}