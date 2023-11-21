using System;

namespace Frogalypse.Settings {
	[Serializable]
	internal struct GraphicsSettings {
		public bool FullScreen;

		public static GraphicsSettings Default() => new() {
			FullScreen = true,
		};
	}
}
