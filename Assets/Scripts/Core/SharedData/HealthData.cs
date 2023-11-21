using System;

using UnityEngine;

namespace Frogalypse.SharedData {
	[CreateAssetMenu(fileName = "HealthData", menuName = "Frogalypse/Shared Data/Health")]
	public class HealthData : ScriptableObject {
		public event Action Died = delegate { };

		[field: SerializeField] public byte Max { get; private set; }
		[field: SerializeField] public byte Current { get; private set; }

		public void Damage(byte amount) {
			Current = (byte) Mathf.Max(Current - amount, 0);
			if (Current == 0)
				Died?.Invoke();
		}

		public void Reset() => Current = Max;
	}
}
