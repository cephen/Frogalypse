using System;
using System.Collections.Generic;

using Frogalypse.Settings;

using SideFX.Scenes;

using UnityEngine;

namespace Frogalypse.Persistence {
	[Serializable]
	internal class Save {
		public FrogalypseSettings Settings { get; private set; } = FrogalypseSettings.Default();
		public Dictionary<GameplaySceneSO, LevelRecord> LevelRecords { get; private set; } = new();

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
			Settings = new FrogalypseSettings {
				Audio = settings.AudioSettings,
				Graphics = settings.GraphicsSettings,
			};
		}

		public string ToJson() => JsonUtility.ToJson(this);
		public void LoadFromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
	}

}
