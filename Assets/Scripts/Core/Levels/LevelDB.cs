using System.Collections.Generic;

using Frogalypse.Persistence;

using SideFX.SceneManagement;

using UnityEngine;

namespace Frogalypse.Levels {
	[CreateAssetMenu(fileName = "LevelDB", menuName = "Frogalypse/Databases/Levels")]
	internal class LevelDB : ScriptableObject {
		[SerializeField] private GameplayScene[] _levels;
		private readonly Dictionary<GameplayScene, LevelRecord> _records = new();

		public int Count => _levels.Length;

		/// <summary>
		/// Indexer that fetches a level reference from an integer level ID
		/// </summary>
		public GameplayScene this[int i] => _levels[i];

		/// <summary>
		/// Indexer that fetches the records for a level from that level's reference
		/// </summary>
		public LevelRecord this[GameplayScene level] {
			get {
				if (!_records.ContainsKey(level)) {
					_records.Add(level, LevelRecord.Default());
				}
				return _records[level];
			}
		}

		private void OnEnable() => SaveSystem.SaveLoadedEvent += OnSaveLoaded;
		private void OnDisable() => SaveSystem.SaveLoadedEvent -= OnSaveLoaded;

		private void OnSaveLoaded(Save save) {
			_records.Clear();
			foreach (GameplayScene level in _levels) {
				string assetId = level.Guid;
				if (save.LevelRecords.TryGetValue(assetId, out LevelRecord record)) {
					_records.Add(level, record);
				} else {
					_records.Add(level, LevelRecord.Default());
				}
			}
		}

		internal void SaveRecord(GameplayScene level, LevelRecord newRecord) {
			if (!_records.ContainsKey(level)) {
				_records.Add(level, newRecord);
				return;
			}

			LevelRecord current = _records[level];

			LevelRecord next = new LevelRecord {
				IsComplete = true,
				BestTime = newRecord.BestTime.TotalMilliseconds < current.BestTime.TotalMilliseconds
					? newRecord.BestTime
					: current.BestTime,
			};

			_records[level] = next;
		}
	}
}
