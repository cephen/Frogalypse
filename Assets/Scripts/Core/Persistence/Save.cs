using System;
using System.Collections.Generic;

using Frogalypse.Settings;

using Newtonsoft.Json;

using SideFX.Scenes;

using UnityEngine;

namespace Frogalypse.Persistence {
	[Serializable]
	internal class Save {
		[SerializeField] private FrogalypseSettings _settings;
		[SerializeField] private Dictionary<GameplaySceneSO, LevelRecord> _levelRecords;

		public FrogalypseSettings Settings => _settings;
		public Dictionary<GameplaySceneSO, LevelRecord> LevelRecords => _levelRecords;


		public Save() {
			_settings = FrogalypseSettings.Default();
			_levelRecords = new Dictionary<GameplaySceneSO, LevelRecord>();
		}

		public void SaveRecord(GameplaySceneSO level, LevelRecord newRecord) {
			if (!LevelRecords.ContainsKey(level))
				LevelRecords.Add(level, LevelRecord.Default());

			LevelRecord current = LevelRecords[level];
			LevelRecord next = new() {
				IsComplete = true,
				BestTime = newRecord.BestTime.TotalMilliseconds < current.BestTime.TotalMilliseconds ? current.BestTime : newRecord.BestTime,
			};

			LevelRecords[level] = next;
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
