using UnityEngine;
using UnityEngine.Events;

namespace SideFX.Events {
	/// <summary>
	/// A component that allows propagation of Events from EventChannels to targets set int he inspector.
	/// </summary>
	public class VoidEventListener : MonoBehaviour {
		[SerializeField] private VoidEventChannelSO _channel = default;
		public UnityEvent OnEventRaised;

		private void OnEnable() {
			if (_channel != null)
				_channel.OnEventRaised += Respond;
		}

		private void OnDisable() {
			if (_channel != null)
				_channel.OnEventRaised -= Respond;
		}

		private void Respond() {
			OnEventRaised?.Invoke();
		}
	}
}
