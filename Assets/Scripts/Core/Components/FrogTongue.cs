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

		private void Awake() {
			_body = GetComponent<Rigidbody2D>();
			_spring = GetComponent<SpringJoint2D>();
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

		#region Event Handlers

		// Anchor Update Handlers
		private void OnPlayerAnchorUpdated() => _player = _playerAnchor.Value;
		private void OnTetherLauncherAnchorUpdated() => _tetherLauncher = _tetherLauncherAnchor.Value;
		private void OnReticleAnchorUpdated() => _reticle = _reticleAnchor.Value;

		// Input Handlers
		private void OnLaunchTether() {
			var startPos = (Vector2) _tetherLauncher.position;
			var target = (Vector2) _reticle.position;
			var delta = target - startPos;
			var direction = delta.normalized;

		}
		private void OnCancelTether() { }

		#endregion

		internal void UpdateSettings(TetherSettings settings) => _settings = settings;

	}
}
