using System;

using UnityEngine;

namespace Frogalypse.Components {
	internal class HealthComponent : MonoBehaviour {
		/// <summary>
		/// Invoked if taking damage would bring this GameObject below zero HitPoints.
		/// </summary>
		public event Action<GameObject> DeathEvent = delegate { };

		[SerializeField] private int _maxHealth = 3;

		internal int CurrentHealth { get; private set; }

		private void Awake() => CurrentHealth = _maxHealth;

		internal void Heal(int amount) => CurrentHealth = Math.Min(CurrentHealth + amount, _maxHealth);

		internal void Damage(int amount) {
			int newHealth = CurrentHealth - amount;
			if (newHealth <= 0) {
				CurrentHealth = 0;
				DeathEvent?.Invoke(gameObject);
			} else {
				CurrentHealth = newHealth;
			}
		}
	}
}
