using Frogalypse.SharedData;

using UnityEngine;

namespace Frogalypse.Components {
	internal class HealthComponent : MonoBehaviour {
		[SerializeField] private HealthData _healthData;

		private void Awake() => _healthData.Reset();

		private void OnEnable() => _healthData.Died += OnDeath;

		private void OnDisable() => _healthData.Died -= OnDeath;

		private void OnDeath() => Debug.Log($"{name} just died! L + skill issue + ratio");

		public void Damage(byte amount) => _healthData.Damage(amount);
	}
}
