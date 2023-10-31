using Frogalypse.Components;
using Frogalypse.Input;
using Frogalypse.Settings;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Rigidbody2D), typeof(ActorMover))]
	public class PlayerController : MonoBehaviour {
		[Header("Assets")]
		[SerializeField] private InputReader _input;
		[SerializeField] private PlayerSettings _playerSettings;
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;
		[SerializeField] private TransformAnchor _tetherStartPointAnchor;

		[Header("Child Transforms")]
		[SerializeField] private Transform _tetherLauncherPivot;
		[SerializeField] private Transform _tetherLauncher;
		[SerializeField] private Transform _tetherStartPoint;

		// Components
		private Rigidbody2D _body;
		private SpringJoint2D _tetherSpring;
		private ActorMover _mover;

		private void Awake() {
			if (_input == null) {
				Debug.LogError($"Input Reader isn't set D:");
				Destroy(this);
			}
			SetPlayerTransformAnchor();

			_body = GetComponent<Rigidbody2D>();
			InitTether();
			InitActorMover();
		}

		private void OnEnable() {
			if (_input != null) {
				_input.TetherEvent += OnGrapple;
				_input.TetherCancelledEvent += OngrappleCancelled;
				_input.MoveEvent += _mover.ProvideInput;
			}
		}

		private void OnDisable() {
			if (_input != null) {
				_input.TetherEvent -= OnGrapple;
				_input.TetherCancelledEvent -= OngrappleCancelled;
				_input.MoveEvent -= _mover.ProvideInput;
			}
		}

		private void SetPlayerTransformAnchor() {
			if (_playerAnchor == null) {
				Debug.LogError("Reference to Player Anchor isn't set D:", _playerAnchor);
				Destroy(this);
			} else {
				_playerAnchor.Provide(transform);
			}
		}

		private void OnGrapple() {
			if (_reticleAnchor == null || !_reticleAnchor.IsSet) {
				Debug.LogError("Reticle Anchor isn't set, can't get grapple target", _reticleAnchor);
				return;
			}
			Debug.DrawLine(transform.position, _reticleAnchor.Value.position, Color.cyan, 2f);
		}

		private void OngrappleCancelled() { }

		private void InitTether() {
			_tetherStartPointAnchor.Provide(_tetherStartPoint);
			// Get reference to or add SpringJoint2D
			_tetherSpring = TryGetComponent(out SpringJoint2D component) ? component : gameObject.AddComponent<SpringJoint2D>();

			_tetherSpring.enabled = false;
			_tetherSpring.enableCollision = false;
			_tetherSpring.autoConfigureDistance = false;
			_tetherSpring.breakForce = Mathf.Infinity;
			_tetherSpring.breakTorque = Mathf.Infinity;
			_tetherSpring.dampingRatio = 0.9f;
		}

		private void InitActorMover() {
			_mover = TryGetComponent(out ActorMover component) ? component : gameObject.AddComponent<ActorMover>();
			_mover.ProvideSettings(_playerSettings.MoveSettings);
		}
	}
}
