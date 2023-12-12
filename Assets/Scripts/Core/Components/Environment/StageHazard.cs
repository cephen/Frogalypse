using Frogalypse.Components;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Collider2D))]
	public class StageHazard : MonoBehaviour {
		private void OnCollisionEnter2D(Collision2D collision) {
			if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out HealthComponent health)) {
				health.Damage(1);
			}
		}
	}
}
