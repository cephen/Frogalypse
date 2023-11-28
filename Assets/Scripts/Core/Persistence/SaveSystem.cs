using System;

using Frogalypse.Levels;
using Frogalypse.Settings;

using SideFX.Events;
using SideFX.SceneManagement;

using UnityEngine;

namespace Frogalypse.Persistence {

	[CreateAssetMenu(fileName = "SaveSystem", menuName = "Frogalypse/Save System")]
	internal class SaveSystem : ScriptableObject {
		internal static event Action<Save> SaveLoadedEvent = delegate { };

		[SerializeField] private GameSettingsSO _gameSettings;
		[SerializeField] private LevelDB _levelDatabase;

		private EventBinding<SaveGameEvent> _saveGameBinding;

		private const string SaveName = "save.frog";
		private const string SaveBackupName = SaveName + ".bak";
		private readonly Save _saveData = new();
		public Save Save => _saveData;

		private void Awake() => LoadSaveDataFromDisk();

		private void OnEnable() {
			_saveGameBinding = new(SaveDataToDisk);
			EventBus<SaveGameEvent>.Register(_saveGameBinding);
		}

		private void OnDisable() {
			EventBus<SaveGameEvent>.Deregister(_saveGameBinding);
		}

		private void SaveSettings() => _saveData.SaveSettings(_gameSettings);

		public void WriteEmptySaveFile() => FileManager.WriteToFile(SaveName, new Save().ToJson());

		public void SaveDataToDisk() {
			_saveData.LevelRecords.Clear();
			for (int i = 0 ; i < _levelDatabase.Count ; i++) {
				GameplayScene level = _levelDatabase[i];
				LevelRecord record = _levelDatabase[level];
				_saveData.SaveRecord(level, record);
			}
			if (FileManager.MoveFile(SaveName, SaveBackupName))
				FileManager.WriteToFile(SaveName, _saveData.ToJson());
		}

		public bool LoadSaveDataFromDisk() {
			FileManager.CreateIfNotExists(SaveName, _saveData.ToJson());

			if (FileManager.LoadFromFile(SaveName, out string json)) {
				_saveData.LoadFromJson(json);
				SaveLoadedEvent?.Invoke(_saveData);
				return true;
			}
			return false;
		}
	}
}
