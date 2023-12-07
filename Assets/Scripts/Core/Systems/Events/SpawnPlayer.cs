using UnityEngine;

using SideFX.Events;

namespace Frogalypse.Events {
	internal readonly struct SpawnPlayer : IEvent {
		internal readonly Vector2 Position;
		internal SpawnPlayer(Vector2 position) => Position = position;
	}
}
