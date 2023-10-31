using Frogalypse.Settings;

using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(Rigidbody2D))]
	public class JumpComponent : MonoBehaviour {
		[SerializeField] private JumpSettings _settings;

		private Rigidbody2D _body;
		private bool _canJump = false;
		private float _inputStartTime;

		private void Awake() {
			_body = TryGetComponent(out Rigidbody2D component) ? component : gameObject.AddComponent<Rigidbody2D>();
			if (_settings == null) {
				Debug.Log("Jump settings not found", _settings);
				enabled = false;
			}
		}


		public void OnJump() {
			_inputStartTime = Time.time;
			Debug.Log($"Jump input started at {_inputStartTime}", this);
		}

		public void OnJumpCancelled() {
			float endTime = Time.time;
			float jumpTime = endTime - _inputStartTime;
			Debug.Log($"Jump input ended at {endTime}, total length {jumpTime}");
		}

		public void ProvideSettings(JumpSettings settings) => _settings = settings;
	}
}
