using System;

namespace Frogalypse.Levels {
	[Serializable]
	internal struct LevelRecord {
		public bool IsComplete { get; internal set; }
		public TimeSpan BestTime { get; internal set; }

		public static LevelRecord Default() => new() {
			IsComplete = false,
			BestTime = TimeSpan.MaxValue,
		};
	}
}
