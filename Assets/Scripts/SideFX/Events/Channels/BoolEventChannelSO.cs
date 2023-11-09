using System;

using UnityEngine;

namespace SideFX.Events {
	/// <summary>
	/// This class is used for Events that involve a boolean (e.g. PlayerWeaponSheatheEvent)
	/// </summary>
	[CreateAssetMenu(fileName = "Bool Event Channel", menuName = "SideFX/Event Channels/Bool Event Channel")]
	public class BoolEventChannelSO : DescriptionBaseSO {
		public event Action<bool> OnEventRaised = delegate { };

		public void RaiseEvent(bool value) {
			OnEventRaised?.Invoke(value);
		}
	}
}
