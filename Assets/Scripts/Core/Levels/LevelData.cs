using System;

using SideFX.SceneManagement;

using UnityEngine;

namespace Frogalypse.Levels {
	[Serializable]
	internal struct LevelData {
		[SerializeField] private GameplayScene _scene;
		[SerializeField] private bool _isCompleted;
		[SerializeField] private TimeSpan _bestTime;

		internal GameplayScene Scene => _scene;
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
