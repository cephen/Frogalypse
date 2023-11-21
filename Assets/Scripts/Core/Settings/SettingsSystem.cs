using Frogalypse.Persistence;

using SideFX.Events;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Frogalypse.Settings {

	public class SettingsSystem : MonoBehaviour {
		[SerializeField] private SaveSystem _saveSystem;
		[SerializeField] private GameSettingsSO _settings;
		[SerializeField] private UniversalRenderPipelineAsset _urpAsset;

		private EventBinding<SaveSettingsEvent> _saveSettingsBinding;

		private void Awake() {
			if (!_saveSystem.LoadSaveDataFromDisk()) {
				_saveSystem.WriteEmptySaveFile();
				_saveSystem.LoadSaveDataFromDisk();
			}
			_settings.LoadSavedSettings(_saveSystem.Save.Settings);
		}

		private void OnEnable() {
			_saveSettingsBinding = new EventBinding<SaveSettingsEvent>(SaveSettings);
			EventBus<SaveSettingsEvent>.Register(_saveSettingsBinding);
		}

		private void OnDisable() => EventBus<SaveSettingsEvent>.Deregister(_saveSettingsBinding);
		private void SaveSettings() => _saveSystem.SaveDataToDisk();
	}
}
