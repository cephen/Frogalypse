using UnityEngine;

namespace Frogalypse.Settings {
	internal class GameSettingsSO : ScriptableObject {
		[SerializeField] private FrogalypseSettings _settings;

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
