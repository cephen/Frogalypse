using Frogalypse.Input;
using Frogalypse.Settings;

using SideFX;

using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(SpringJoint2D))]
	public class FrogTongue : MonoBehaviour {
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
			Vector2 startPos = (Vector2) _tetherLauncher.position;
			Vector2 target = (Vector2) _reticle.position;
			Vector2 delta = target - startPos;
			Vector2 direction = delta.normalized;
			float distance = Mathf.Min(delta.magnitude, _settings.maxTravelDistance);

			Vector2 velocity = direction * (distance / _settings.travelTime);

			Debug.Log("Launching Tether.\n" +
				$"Start Position: {startPos}\n" +
				$"Target: {target}\n" +
				$"Delta: {delta}\n" +
				$"Direction: {direction}\n" +
				$"Distance: {distance}\n" +
				$"Velocity: {velocity}");

			_body.velocity = velocity;

		}
		private void OnCancelTether() { }

		#endregion

		internal void UpdateSettings(TetherSettings settings) => _settings = settings;

	}
}
