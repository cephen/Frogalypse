using System;

using Frogalypse.Settings;

using UnityEngine;

namespace Frogalypse.Persistence {
	[Serializable]
	internal class Save {
		public FrogalypseSettings FrogalypseSettings { get; private set; } = FrogalypseSettings.Default();


		public void SaveSettings(FrogalypseSettings settings) {
			FrogalypseSettings = settings;
		}

		public string ToJson() => JsonUtility.ToJson(this);
		public void LoadFromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
	}

}
