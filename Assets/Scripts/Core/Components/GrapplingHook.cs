﻿using System;

using Frogalypse.Settings;

using SideFX;

using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(SpringJoint2D))]
	public class GrapplingHook : MonoBehaviour {
		/// <summary>
		/// Invoked when the hook hits a tetherable object
		/// </summary>
		public event Action HitEvent = delegate { };

		/// <summary>
		/// Invoked when the hook reaches the max distance from the player
		/// </summary>
		public event Action MissEvent = delegate { };

		[SerializeField] private TransformAnchor _playerTransform;
		[SerializeField] private TransformAnchor _reticleTransform;
		[SerializeField] private TransformAnchor _hookLauncherTransform;

		private Rigidbody2D _body;
		private Rigidbody2D _playerBody;
		private SpringJoint2D _spring;
		private TetherSettings _settings;
		private bool _isTravelling = false;

		private void Awake() {
			_body = GetComponent<Rigidbody2D>();
			_spring = GetComponent<SpringJoint2D>();
		}

		private void OnEnable() {
			_playerTransform.OnAnchorUpdated += OnPlayerAnchorSet;
			ResetSpring();
		}

		private void OnDisable() {
			_playerTransform.OnAnchorUpdated -= OnPlayerAnchorSet;
			ResetSpring();
		}

		private void FixedUpdate() {
			if (!_isTravelling)
				return;

			if (!_playerTransform.IsSet) {
				Debug.LogError("Player Transform anchor isn't set D:");
				ResetSpring();
				enabled = false;
				return;
			}

			Vector2 startPos = (Vector2) _hookLauncherTransform.Value.position;

			// Early exit if tether length is beyond maximum
			if (Vector2.Distance(startPos, _body.position) > _settings.maxTravelDistance) {
				ResetSpring();
				MissEvent?.Invoke();
				return;
			}

			// Since contacts are filtered, only consider the first object in the way
			RaycastHit2D[] hitBuffer = new RaycastHit2D[1];
			int numHits = Physics2D.CircleCast(_body.position, 0.3f, _body.velocity.normalized, _settings.contactFilter, hitBuffer);

			for (int i = 0 ; i < numHits ; i++) {
				RaycastHit2D hit = hitBuffer[i];
				float totalDistance = Vector2.Distance(startPos, hit.point);

				if (totalDistance < _settings.maxTravelDistance) {
					Debug.Log($"Attaching hook to object {hit.collider.name}");
					_body.MovePosition(hit.point);
					_body.velocity = Vector2.zero;

					_spring.connectedBody = _playerBody;
					_spring.distance = Mathf.Min(_settings.targetLength, totalDistance);
					_spring.enabled = true;

					_isTravelling = false;
					HitEvent?.Invoke();
					return;
				}
			}
		}

		public void UpdateSettings(TetherSettings settings) => _settings = settings;
		public void OnPlayerAnchorSet() => _playerBody = _playerTransform.Value.GetComponent<Rigidbody2D>();

		public void Fire() {
			if (_isTravelling)
				return;

			Vector2 startPos = (Vector2) _hookLauncherTransform.Value.position;
			Vector2 reticlePosition = (Vector2) _reticleTransform.Value.position;
			Vector2 delta = reticlePosition - startPos;
			Vector2 direction = delta.normalized;

			transform.position = startPos;

			RaycastHit2D[] hits = new RaycastHit2D[4];
			int numHits = Physics2D.Raycast(startPos, direction, _settings.contactFilter, hits);

			float distance = numHits switch {
				0 => _settings.maxTravelDistance,
				_ => Mathf.Min(_settings.maxTravelDistance, delta.magnitude),
			};

			float speed = distance / _settings.travelTime;
			_body.velocity = direction * speed;
			_isTravelling = true;
		}

		public void Reload() {
			transform.localPosition = Vector2.zero;
			_body.velocity = Vector2.zero;
			_isTravelling = false;

			ResetSpring();
		}

		private void ResetSpring() {
			_spring.enabled = false;
			_spring.connectedBody = null;
		}
	}
}
