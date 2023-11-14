using System;

namespace Frogalypse.Settings {
	[Serializable]
	internal struct AudioSettings {
		public float MasterVolume;
		public float EffectsVolume;
		public float MusicVolume;

		public static AudioSettings Default() => new() {
			MasterVolume = 1f,
			EffectsVolume = 1f,
			MusicVolume = 1f,
		};
	}
}
