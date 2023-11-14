using System;

using Frogalypse.Settings;

using UnityEngine;

namespace Frogalypse.Persistence {
	[Serializable]
	internal class Save {
		public FrogalypseSettings Settings { get; private set; } = FrogalypseSettings.Default();


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
