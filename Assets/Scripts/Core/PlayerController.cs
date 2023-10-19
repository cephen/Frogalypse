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
			if ( _activeGrapple != null ) return;

			Ray ray = Camera.main.ScreenPointToRay(_input.MousePosition);

			Debug.DrawLine(ray.origin, ray.origin + ray.direction * 20f, Color.green, 1f);

			RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, 20f, GrappleableLayers.value);

			if ( hits.Length > 0 ) {
				Debug.Log($"Grapple check hit {hits.Length} objects");
				var point = hits[0].point;
				if ( Vector2.Distance(transform.position, point) <= MaxGrappleDistance ) {
					Debug.Log($"Spawing Grapple Point at ({point.x}, {point.y})");
					_activeGrapple = Instantiate(_grapplePrefab, point, Quaternion.identity);
					_activeGrapple.ConnectPlayer(_rb);
				}
			}
		}

		private void OngrappleCancelled() {
			Destroy(_activeGrapple.gameObject);
			_activeGrapple = null;
		}
	}
}
