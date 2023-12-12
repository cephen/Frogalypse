using System;

using Frogalypse.Data;

using UnityEngine;

namespace Frogalypse.Components {
	internal class HealthComponent : MonoBehaviour {
		[field: SerializeField] internal ByteVariable MaxHealth { get; private set; }
		internal byte CurrentHealth { get; private set; }

		internal event Action Died = delegate { };
		internal event Action HealthChanged = delegate { };

		private void Awake() => CurrentHealth = MaxHealth.Value;

		private void OnEnable() {
			HealthChanged += OnHealthChanged;
			Died += OnDeath;
		}

		private void OnDisable() {
			HealthChanged -= OnHealthChanged;
			Died -= OnDeath;
		}

		public void Damage(byte amount) {
			CurrentHealth = (byte) Mathf.Max(CurrentHealth - amount, 0);
			HealthChanged?.Invoke();
			if (CurrentHealth == 0)
				Died?.Invoke();
		}

		private void OnDeath() => Debug.Log($"{name} just died! L + skill issue + ratio");
		private void OnHealthChanged() => Debug.Log($"{name}'s health changed: {CurrentHealth}");
	}
}
