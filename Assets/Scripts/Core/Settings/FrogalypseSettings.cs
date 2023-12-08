using System;

namespace Frogalypse.Settings {
	[Serializable]
	internal struct FrogalypseSettings {
		public AudioSettings Audio;
		public GraphicsSettings Graphics;

		public static FrogalypseSettings Default() => new() {
			Audio = AudioSettings.Default(),
			Graphics = GraphicsSettings.Default(),
		};
	}

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

	[Serializable]
	internal struct GraphicsSettings {
		public bool FullScreen;

		public static GraphicsSettings Default() => new() {
			FullScreen = true,
		};
	}
}
