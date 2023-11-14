using System.Collections.Generic;

using Frogalypse.Persistence;

using SideFX.Scenes;

using UnityEngine;

namespace Frogalypse.Levels {
	[CreateAssetMenu(fileName = "LevelDB", menuName = "Frogalypse/Databases/Levels")]
	internal class LevelDB : ScriptableObject {
		[SerializeField] private GameplaySceneSO[] _levels;
		[SerializeField] private SaveSystem _saveSystem;
		private readonly Dictionary<GameplaySceneSO, LevelRecord> _records = new();

		public int Count => _levels.Length;
		public GameplaySceneSO this[int i] => _levels[i];
		public LevelRecord this[GameplaySceneSO level] {
			get {
				if (!_records.ContainsKey(level)) {
					_records.Add(level, LevelRecord.Default());
				}
				return _records[level];
			}
		}

		private void OnEnable() => _saveSystem.SaveLoadedEvent += OnSaveLoaded;
		private void OnDisable() => _saveSystem.SaveLoadedEvent -= OnSaveLoaded;

		private void OnSaveLoaded(Save save) {
			_records.Clear();
			foreach (GameplaySceneSO level in _levels) {
				if (save.LevelRecords.TryGetValue(level, out LevelRecord record)) {
					_records.Add(level, record);
				} else {
					_records.Add(level, LevelRecord.Default());
				}
			}
		}
	}
}
