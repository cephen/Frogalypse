using Frogalypse.Input;
using Frogalypse.Settings;

using Shapes;

using SideFX.Anchors;

using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(SpringJoint2D), typeof(Line))]
	internal sealed class FrogTongue : MonoBehaviour {
		[SerializeField] private InputReader _input;
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private TransformAnchor _tetherLauncherAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;

		private Transform _playerTransform;
		private Transform _tetherLauncher;
		private Transform _reticle;
		private TetherSettings _settings;
		private SpringJoint2D _spring;
		private Rigidbody2D _body;
		private Rigidbody2D _playerBody;
		private Line _line;

		private State _state = State.Ready;
		private Vector2 _anchorPoint;

		private enum State {
			Ready, Travelling, Anchored
		}

		#region Unity Lifecycle

		private void Awake() {
			_body = GetComponent<Rigidbody2D>();
			_spring = GetComponent<SpringJoint2D>();
			_line = GetComponent<Line>();
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

		private void Update() {
			// Start of the line tracks the player
			if (_state is not State.Ready) {
				Vector2 playerHookPositionDelta = _playerTransform.position - transform.position;
				Vector2 direction = playerHookPositionDelta.normalized;
				float distance = playerHookPositionDelta.magnitude;
				_line.Start = direction * distance;
			}

			// Prevents the end of the line from moving around with the anchor point of the spring joint
			if (_state is State.Anchored) {
				Vector2 anchorPositionDelta = _anchorPoint - (Vector2) transform.position;
				Vector2 direction = anchorPositionDelta.normalized;
				float distance = anchorPositionDelta.magnitude;
				_line.End = direction * distance;
			}
		}

		private void FixedUpdate() {
			if (_state is State.Travelling) {
				// Cancel ability when tip is too far from player
				if (Vector2.Distance(_tetherLauncher.position, _body.position) > _settings.MaxTravelDistance) {
					MakeReady();
					return;
				}

				// Check ahead for collisions
				// Filters are used, so only consider the first valid object
				var hits = new RaycastHit2D[1];
				Vector2 direction = _body.velocity.normalized;
				float checkDistance = _body.velocity.magnitude * Time.deltaTime;

				// TODO: Move circle check radius into tether settings
				if (Physics2D.CircleCast(_body.position, 0.3f, direction, _settings.ContactFilter, hits, checkDistance) > 0)
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
			_line.enabled = false;
			_line.Start = Vector3.zero;
			_line.End = Vector3.zero;
			_state = State.Ready;
		}

		private void MakeAnchored(RaycastHit2D hit) {
			Debug.Log($"Anchoring to object {hit.collider.name} at position {hit.point}. Distance to player: {Vector2.Distance(_body.position, _playerBody.position)}m");
			_body.velocity = Vector2.zero;
			_body.bodyType = RigidbodyType2D.Static;
			transform.position = hit.point;
			_anchorPoint = hit.point;
			_spring.connectedBody = _playerBody;
			_spring.distance = Mathf.Min(Vector2.Distance(_body.position, _tetherLauncher.position), _settings.TargetLength);
			_spring.enabled = true;
			_state = State.Anchored;
		}

		#region Event Handlers

		// Anchor Update Handlers
		private void OnPlayerAnchorUpdated() {
			_playerTransform = _playerAnchor.Value;
			_playerBody = _playerTransform.GetComponent<Rigidbody2D>();
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
			float distance = Mathf.Min(delta.magnitude, _settings.MaxTravelDistance);
			Vector2 velocity = direction * (distance / _settings.TravelTime);

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
			_line.enabled = true;
			_state = State.Travelling;
		}

		private void OnCancelTether() {
			if (_state is not State.Ready)
				MakeReady();
		}

		#endregion
	}
}
