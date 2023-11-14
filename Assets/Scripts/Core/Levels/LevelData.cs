using System;

using SideFX.Scenes;

using UnityEngine;

namespace Frogalypse.Levels {
	[Serializable]
	internal struct LevelData {
		[SerializeField] private GameplaySceneSO _scene;
		[SerializeField] private bool _isCompleted;
		[SerializeField] private TimeSpan _bestTime;

		internal readonly GameplaySceneSO Scene => _scene;
		internal readonly bool IsCompleted => _isCompleted;
		internal readonly TimeSpan BestTime => _bestTime;

		internal void MarkComplete() => _isCompleted = true;
		internal void RecordTime(TimeSpan timeTaken) {
			if (timeTaken.TotalMilliseconds < _bestTime.TotalMilliseconds) {
				_bestTime = timeTaken;
			}
		}
	}
}
