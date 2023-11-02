using Frogalypse.Settings;

using SideFX;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Frogalypse {
	[RequireComponent(typeof(SpriteRenderer))]
	public class Reticle : MonoBehaviour {
		[Header("Settings")]
		[SerializeField] private PlayerSettings _playerSettings;

		[Header("Anchors")]
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;

		// Components
		private Camera _camera;
		private SpriteRenderer _renderer;

		// Data
		private readonly RaycastHit2D[] _targets = new RaycastHit2D[4];

		private void Awake() {
			InitRenderer();
			_camera = Camera.main;

		}

		private void Start() {
			if (_reticleAnchor == null) {
				Debug.LogError("Reticle Anchor isn't set", _reticleAnchor);
				return;
			}
			_reticleAnchor.Provide(transform);
		}

		private void Update() {

			if (!_playerAnchor.IsSet) {
				Debug.LogError("Reference to Player Anchor is not set");
				return;
			}

			(bool targetFound, Vector2 targetPosition) = FindTarget();
			_renderer.color = targetFound ? Color.green : Color.red;
			transform.position = targetPosition;
		}

		private void InitRenderer() {
			_renderer = GetComponent<SpriteRenderer>();
			_renderer.enabled = true;
		}

		/// <summary>
		///	Finds the position to place the reticle, and whether a grappleable target is within range of the player.
		/// </summary>
		/// <returns>
		/// A (bool, Vector2) tuple.
		/// The bool represents whether a grapple target is in range
		/// The Vector2 contains the screen position to place the reticle at
		/// </returns>
		private (bool, Vector2) FindTarget() {
			Vector2 mousePos = (Vector2) _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			Vector2 playerPos = (Vector2) _playerAnchor.Value.position;
			Vector2 positionDelta = mousePos - playerPos;
			Vector2 aimDirection = positionDelta.normalized;

			int numHits = Physics2D.CircleCast(playerPos, 0.3f, aimDirection, _playerSettings.grappleContactFilter, _targets);

			for (int i = 0 ; i < numHits ; i++) {
				RaycastHit2D hit = _targets[i];
				if (hit.distance < _playerSettings.maxGrappleDistance) {
					Debug.DrawLine(playerPos, hit.point, Color.green);
					// If the mouse is further away than the hit point
					return positionDelta.magnitude > hit.distance
						// place the reticle at the hit point
						? (true, hit.point)
						// else place the reticle at the cursor
						: (true, mousePos);
				}
			}

			// This code is only reached if there are no valid targets
			Debug.DrawLine(playerPos, playerPos + aimDirection * _playerSettings.maxGrappleDistance, Color.red);
			return (false, mousePos);
		}
	}
}
