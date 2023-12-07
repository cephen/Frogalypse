using SideFX.Events;

using UnityEngine;

namespace Frogalypse.Levels {
	[RequireComponent(typeof(Collider2D))]
	public class GoalZone : MonoBehaviour {
		private void OnTriggerEnter2D(Collider2D other) {
			if (other.CompareTag("Player")) {
				Debug.Log("Goal Reached!", this);
				EventBus<GoalReached>.Raise(default);
			}
		}
	}

	public readonly struct GoalReached : IEvent { }
}
