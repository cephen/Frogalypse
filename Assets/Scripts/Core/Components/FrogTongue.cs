using Frogalypse.Input;
using Frogalypse.Settings;

using SideFX;

using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(SpringJoint2D))]
	internal class FrogTongue : MonoBehaviour {
		[SerializeField] private InputReader _input;
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private TransformAnchor _tetherLauncherAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;

		private Transform _player;
		private Transform _tetherLauncher;
		private Transform _reticle;
		private TetherSettings _settings;
		private SpringJoint2D _spring;
		private Rigidbody2D _body;
		private Rigidbody2D _playerBody;

		private State _state = State.Ready;

		private enum State {
			Ready, Travelling, Anchored
		}

		#region Unity Lifecycle

		private void Awake() {
			_body = GetComponent<Rigidbody2D>();
			_spring = GetComponent<SpringJoint2D>();
		}

		private void Start() {
			if (_playerAnchor.IsSet)
				OnPlayerAnchorUpdated();

			if (_tetherLauncherAnchor.IsSet)
				OnTetherLauncherAnchorUpdated();

			if (_reticleAnchor.IsSet)
				OnReticleAnchorUpdated();

			MakeReady();
		}

		private void OnEnable() {
			_playerAnchor.OnAnchorUpdated += OnPlayerAnchorUpdated;
			_tetherLauncherAnchor.OnAnchorUpdated += OnTetherLauncherAnchorUpdated;
			_reticleAnchor.OnAnchorUpdated += OnReticleAnchorUpdated;
			_input.TetherEvent += OnLaunchTether;
			_input.TetherCancelledEvent += OnCancelTether;
		}

		private void OnDisable() {
			_playerAnchor.OnAnchorUpdated -= OnPlayerAnchorUpdated;
			_tetherLauncherAnchor.OnAnchorUpdated -= OnTetherLauncherAnchorUpdated;
			_reticleAnchor.OnAnchorUpdated -= OnReticleAnchorUpdated;
			_input.TetherEvent -= OnLaunchTether;
			_input.TetherCancelledEvent -= OnCancelTether;
		}

		private void FixedUpdate() {
			if (_state is State.Travelling) {
				// Cancel ability when tip is too far from player
				if (Vector2.Distance(_tetherLauncher.position, _body.position) > _settings.maxTravelDistance) {
					MakeReady();
					return;
				}

				// Check ahead for collisions
				// Filters are used, so only consider the first valid object
				var hits = new RaycastHit2D[1];
				Vector2 direction = _body.velocity.normalized;
				float checkDistance = _body.velocity.magnitude * Time.deltaTime;

				// TODO: Move circle check radius into tether settings
				if (Physics2D.CircleCast(_body.position, 0.3f, direction, _settings.contactFilter, hits, checkDistance) > 0)
					MakeAnchored(hits[0]);
			}
		}

		#endregion

		internal void UpdateSettings(TetherSettings settings) => _settings = settings;

		private void MakeReady() {
			_body.simulated = false;
			_body.bodyType = RigidbodyType2D.Dynamic;
			_body.velocity = Vector2.zero;
			_spring.enabled = false;
			_spring.connectedBody = null;
			_spring.distance = 0;
			_spring.autoConfigureDistance = false;
			_state = State.Ready;
		}

		private void MakeAnchored(RaycastHit2D hit) {
			Debug.Log($"Anchoring to object {hit.collider.name} at position {hit.point}. Distance to player: {Vector2.Distance(_body.position, _playerBody.position)}m");
			_body.velocity = Vector2.zero;
			_body.bodyType = RigidbodyType2D.Static;
			transform.position = hit.point;
			_spring.connectedBody = _playerBody;
			_spring.distance = Mathf.Min(Vector2.Distance(_body.position, _tetherLauncher.position), _settings.targetLength);
			_spring.enabled = true;
			_state = State.Anchored;
		}

		#region Event Handlers

		// Anchor Update Handlers
		private void OnPlayerAnchorUpdated() {
			_player = _playerAnchor.Value;
			_playerBody = _player.GetComponent<Rigidbody2D>();
		}

		private void OnTetherLauncherAnchorUpdated() => _tetherLauncher = _tetherLauncherAnchor.Value;
		private void OnReticleAnchorUpdated() => _reticle = _reticleAnchor.Value;

		// Input Handlers
		private void OnLaunchTether() {
			if (_state is not State.Ready)
				return;

			Vector2 startPos = (Vector2) _tetherLauncher.position;
			Vector2 target = (Vector2) _reticle.position;
			Vector2 delta = target - startPos;
			Vector2 direction = delta.normalized;
			float distance = Mathf.Min(delta.magnitude, _settings.maxTravelDistance);
			Vector2 velocity = direction * (distance / _settings.travelTime);

#if UNITY_EDITOR
			Debug.Log("Launching Tether\n" +
				$"Start Position: {startPos}\n" +
				$"Target Position: {target}\n" +
				$"Direction: {direction}\n" +
				$"Distance: {distance}\n" +
				$"Velocity: {velocity}");
			Debug.DrawLine(startPos, startPos + direction, Color.magenta, 0.5f);
#endif

			transform.position = startPos;
			_body.velocity = velocity;
			_body.simulated = true;
			_state = State.Travelling;
		}

		private void OnCancelTether() {
			if (_state is not State.Ready)
				MakeReady();
		}

		#endregion


	}
}
