using System;
using System.Collections.Generic;

using Frogalypse.Levels;
using Frogalypse.Settings;

using Newtonsoft.Json;

using SideFX.SceneManagement;

using UnityEngine;

namespace Frogalypse.Persistence {
	[Serializable]
	internal class Save {
		[SerializeField] private FrogalypseSettings _settings;
		[SerializeField] private Dictionary<string, LevelRecord> _levelRecords;

		public FrogalypseSettings Settings => _settings;
		public Dictionary<string, LevelRecord> LevelRecords => _levelRecords;

		public Save() {
			_settings = FrogalypseSettings.Default();
			_levelRecords = new Dictionary<string, LevelRecord>();
		}

		public void SaveRecord(GameplayScene level, LevelRecord newRecord) {
			string levelID = level.Guid; // The AssetDatabase ID of the level's scene file

			if (!LevelRecords.ContainsKey(levelID)) {
				LevelRecords.Add(levelID, newRecord);
				return;
			}

			LevelRecord current = LevelRecords[levelID];
			LevelRecord toSave = new() {
				IsComplete = true,
				BestTime = newRecord.BestTime < current.BestTime
					? current.BestTime
					: newRecord.BestTime,
			};

			LevelRecords[levelID] = toSave;
		}

		public void SaveSettings(GameSettingsSO settings) {
			_settings.Audio = settings.AudioSettings;
			_settings.Graphics = settings.GraphicsSettings;
		}

		public string ToJson() => JsonConvert.SerializeObject(this);

		public void LoadFromJson(string json) {
			Save deserialized = JsonConvert.DeserializeObject<Save>(json);
			_settings = deserialized.Settings;
			_levelRecords.Clear();
			foreach (var (level, record) in deserialized.LevelRecords)
				_levelRecords.Add(level, record);
		}
	}

}
