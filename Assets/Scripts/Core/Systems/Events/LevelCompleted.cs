using System;

using SideFX.Events;
using SideFX.SceneManagement;

namespace Frogalypse.Levels {
	internal readonly struct LevelCompleted : IEvent {
		public readonly TimeSpan TimeTaken { get; init; }
		public readonly GameplayScene LevelScene { get; init; }
	}
}
