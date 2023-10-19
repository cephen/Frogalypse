using System.Collections;
using System.Collections.Generic;

using Frogalypse.Input;
using Frogalypse.Settings;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController : MonoBehaviour {
		[Header("Assets")]
		[SerializeField] private InputReader _input;
		[SerializeField] private PlayerSettings _playerSettings;
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;

		private Rigidbody2D _rb;

		private void Awake() {
			if (_input == null) {
				Debug.LogError($"Input Reader isn't set D:");
				Destroy(this);
			}
			if (_playerAnchor == null) {
				Debug.LogError("Reference to Player Anchor isn't set D:", _playerAnchor);
				Destroy(this);
			} else {
				_playerAnchor.Provide(transform);
			}

			_rb = GetComponent<Rigidbody2D>();
		}

		private void OnEnable() {
			if (_input != null) {
				_input.GrappleEvent += OnGrapple;
				_input.GrappleCancelledEvent += OngrappleCancelled;
			}
		}

		private void OnDisable() {
			if (_input != null) {
				_input.GrappleEvent -= OnGrapple;
				_input.GrappleCancelledEvent -= OngrappleCancelled;
			}
		}

		private void OnGrapple() {
			if (_reticleAnchor == null || !_reticleAnchor.IsSet) {
				Debug.LogError("Reticle Anchor isn't set, can't get grapple target", this);
				return;
			}
			Debug.DrawLine(transform.position, _reticleAnchor.Value.position, Color.cyan, 2f);
		}

		private void OngrappleCancelled() {

		}
	}
}
