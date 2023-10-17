using System.Collections;
using System.Collections.Generic;

using Frogalypse.Input;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController : MonoBehaviour {
		[SerializeField] private InputReader _input;
		private Rigidbody2D _rb;
		public LayerMask GrappleableLayers;
		public float MaxGrappleDistance = 10f;

		[SerializeField] private HingeGrapple _grapplePrefab;
		private HingeGrapple _activeGrapple;

		private void Awake() {
			if ( _input == null ) {
				Debug.LogError($"Input reader isn't set!!");
				Destroy(this);
			}

			_rb = GetComponent<Rigidbody2D>();
		}

		private void OnEnable() {
			if ( _input != null ) {
				_input.GrappleEvent += OnGrapple;
				_input.GrappleCancelledEvent += OngrappleCancelled;
			}
		}

		private void OnDisable() {
			if ( _input != null ) {
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
