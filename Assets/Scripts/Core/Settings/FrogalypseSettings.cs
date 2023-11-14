namespace Frogalypse.Settings {
	internal struct FrogalypseSettings {
		public AudioSettings Audio;
		public GraphicsSettings Graphics;

		public static FrogalypseSettings Default() => new() {
			Audio = AudioSettings.Default(),
			Graphics = GraphicsSettings.Default(),
		};
	}
}
