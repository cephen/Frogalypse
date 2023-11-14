using Frogalypse.Persistence;

using SideFX.Events;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Frogalypse.Settings {
	public class SettingsSystem : MonoBehaviour {
		[SerializeField] private VoidEventChannelSO _saveSettingsEvent;
		[SerializeField] private SaveSystem _saveSystem;
		[SerializeField] private GameSettingsSO _settings;
		[SerializeField] private UniversalRenderPipelineAsset _urpAsset;

		private void Awake() {
			_saveSystem.LoadSaveDataFromDisk();
			_settings.LoadSavedSettings(_saveSystem.Save.Settings);
		}

		private void OnEnable() => _saveSettingsEvent.OnEventRaised += SaveSettings;
		private void OnDisable() => _saveSettingsEvent.OnEventRaised -= SaveSettings;
		private void SaveSettings() => _saveSystem.SaveDataToDisk();
	}
}
