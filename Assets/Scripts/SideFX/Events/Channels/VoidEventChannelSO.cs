using System;

using UnityEngine;

namespace SideFX.Events {
	/// <summary>
	/// This class is used for Events that have no arguments (e.g. exit game event)
	/// </summary>
	[CreateAssetMenu(fileName = "Void Event Channel", menuName = "SideFX/Event Channels/Void Event Channel")]
	public class VoidEventChannelSO : DescriptionBaseSO {
		public event Action OnEventRaised = delegate { };

		public void RaiseEvent() {
			OnEventRaised?.Invoke();
		}
	}
}
