using UnityEngine;

namespace Frogalypse.Settings {
	internal class GameSettingsSO : ScriptableObject {
		[SerializeField] private FrogalypseSettings _settings;

		public void LoadSavedSettings(FrogalypseSettings settings) {
			AudioSettings = settings.Audio;
			GraphicsSettings = settings.Graphics;
		}

		public AudioSettings AudioSettings {
			get => _settings.Audio;
			set => _settings.Audio = value;
		}

		public GraphicsSettings GraphicsSettings {
			get => _settings.Graphics;
			set => _settings.Graphics = value;
		}
	}
}
