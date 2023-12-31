﻿using System;

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
		private EventBinding<LevelCompleted> _levelCompleteBinding;

		private const string SaveName = "save.frog";
		private const string SaveBackupName = SaveName + ".bak";
		private readonly Save _saveData = new();
		public Save Save => _saveData;

		private void Awake() => LoadSaveDataFromDisk();

		private void OnEnable() {
			_saveGameBinding = new EventBinding<SaveGameEvent>(SaveDataToDisk);
			_levelCompleteBinding = new EventBinding<LevelCompleted>(OnLevelCompleted);
			EventBus<SaveGameEvent>.Register(_saveGameBinding);
			EventBus<LevelCompleted>.Register(_levelCompleteBinding);
		}

		private void OnDisable() {
			EventBus<SaveGameEvent>.Deregister(_saveGameBinding);
			EventBus<LevelCompleted>.Deregister(_levelCompleteBinding);
		}

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
			FileManager.CreateIfNotExists(SaveName, new Save().ToJson());

			if (FileManager.LoadFromFile(SaveName, out string json)) {
				_saveData.LoadFromJson(json);
				SaveLoadedEvent?.Invoke(_saveData);
				return true;
			}
			return false;
		}

		private void OnLevelCompleted(LevelCompleted @event) {
			Debug.Log($"Saving level record {@event.LevelScene} - {@event.TimeTaken}");

			_levelDatabase.SaveRecord(@event.LevelScene, new LevelRecord {
				IsComplete = true,
				BestTime = @event.TimeTaken,
			});

			SaveDataToDisk();
		}
	}
}
