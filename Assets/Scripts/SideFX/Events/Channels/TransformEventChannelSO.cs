using System;

using UnityEngine;

namespace SideFX.Events {
	/// <summary>
	/// This class is used for Events that involve a Transform (e.g. player spawned event)
	/// </summary>
	[CreateAssetMenu(fileName = "Transform Event Channel", menuName = "SideFX/Event Channels/Transform Event Channel")]
	public class TransformEventChannelSO : DescriptionBaseSO {
		public event Action<Transform> OnEventRaised = delegate { };

		public void RaiseEvent(Transform transform) {
			OnEventRaised?.Invoke(transform);
		}
	}
}
