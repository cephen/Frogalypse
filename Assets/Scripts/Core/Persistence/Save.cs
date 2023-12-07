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
			string levelID = level.Guid;
			if (!LevelRecords.ContainsKey(levelID)) {
				LevelRecords.Add(levelID, newRecord);
				return;
			}

			LevelRecord current = LevelRecords[levelID];
			LevelRecord next = new() {
				IsComplete = newRecord.IsComplete,
				BestTime = newRecord.BestTime.TotalMilliseconds < current.BestTime.TotalMilliseconds ? current.BestTime : newRecord.BestTime,
			};

			LevelRecords[levelID] = next;
		}

		public void SaveSettings(GameSettingsSO settings) {
			_settings = new FrogalypseSettings {
				Audio = settings.AudioSettings,
				Graphics = settings.GraphicsSettings,
			};
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
