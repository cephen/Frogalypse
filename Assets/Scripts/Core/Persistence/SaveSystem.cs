using System;

using Frogalypse.Settings;

using SideFX.Events;

using UnityEngine;

namespace Frogalypse.Persistence {
	[CreateAssetMenu(fileName = "SaveSystem", menuName = "Frogalypse/Save System")]
	internal class SaveSystem : ScriptableObject {
		internal event Action<Save> SaveLoadedEvent = delegate { };

		[SerializeField] private VoidEventChannelSO _saveSettingsEvent;
		[SerializeField] private VoidEventChannelSO _saveGameEvent;
		[SerializeField] private GameSettingsSO _gameSettings;

		private const string SaveName = "save.frog";
		private const string SaveBackupName = SaveName + ".bak";
		private readonly Save _saveData = new();
		public Save Save => _saveData;

		private void OnEnable() {
			_saveSettingsEvent.OnEventRaised += SaveSettings;
			_saveGameEvent.OnEventRaised += SaveDataToDisk;
		}

		private void OnDisable() {
			_saveSettingsEvent.OnEventRaised -= SaveSettings;
			_saveGameEvent.OnEventRaised -= SaveDataToDisk;
		}

		private void SaveSettings() => _saveData.SaveSettings(_gameSettings);

		public void WriteEmptySaveFile() => FileManager.WriteToFile(SaveName, string.Empty);

		public void SaveDataToDisk() {
			if (FileManager.MoveFile(SaveName, SaveBackupName))
				FileManager.WriteToFile(SaveName, _saveData.ToJson());
		}

		public bool LoadSaveDataFromDisk() {
			if (FileManager.LoadFromFile(SaveName, out string json)) {
				_saveData.LoadFromJson(json);
				SaveLoadedEvent?.Invoke(_saveData);
				return true;
			}
			return false;
		}
	}
}
