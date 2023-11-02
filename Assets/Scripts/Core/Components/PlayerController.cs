using Frogalypse.Components;
using Frogalypse.Input;
using Frogalypse.Settings;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(MoveComponent), typeof(JumpComponent))]
	public class PlayerController : MonoBehaviour {
		[Header("Assets")]
		[SerializeField] private InputReader _input;
		[SerializeField] private PlayerSettings _playerSettings;
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;
		[SerializeField] private TransformAnchor _tetherStartPointAnchor;
		[SerializeField] private TransformAnchor _mainCameraAnchor;

		[Header("Child Transforms")]
		[SerializeField] private Transform _tetherLauncherPivot;
		[SerializeField] private Transform _tetherLauncher;
		[SerializeField] private Transform _tetherStartPoint;

		// Components
		private SpringJoint2D _tetherSpring;
		private MoveComponent _mover;
		private JumpComponent _jump;
		private Camera _camera;

		private void Awake() {
			if (_input == null) {
				Debug.LogError($"Input Reader isn't set D:", _input);
				return;
			}

			InitTether();
			InitMoveComponent();
			InitJumpComponent();
			SetPlayerTransformAnchor();
		}

		private void Start() {
			if (_mainCameraAnchor.IsSet) {
				UpdateCameraReference();
			}
		}

		private void OnEnable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set D:", _input);
				return;
			}
			_input.MoveEvent += _mover.ProvideInput;
			_input.JumpEvent += _jump.OnJump;
			_input.JumpCancelledEvent += _jump.OnJumpCancelled;
			_mainCameraAnchor.OnAnchorUpdated += UpdateCameraReference;
		}

		private void OnDisable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set D:", _input);
				return;
			}
			_input.MoveEvent -= _mover.ProvideInput;
			_input.JumpEvent -= _jump.OnJump;
			_input.JumpCancelledEvent -= _jump.OnJumpCancelled;
			_mainCameraAnchor.OnAnchorUpdated -= UpdateCameraReference;
		}

		private void SetPlayerTransformAnchor() {
			if (_playerAnchor == null) {
				Debug.LogError("Reference to Player Anchor isn't set D:", _playerAnchor);
				Destroy(this);
			} else {
				_playerAnchor.Provide(transform);
			}
		}

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

		private void InitMoveComponent() {
			_mover = TryGetComponent(out MoveComponent component) ? component : gameObject.AddComponent<MoveComponent>();
			_mover.ProvideSettings(_playerSettings.MoveSettings);
		}

		private void InitJumpComponent() {
			_jump = TryGetComponent(out JumpComponent component) ? component : gameObject.AddComponent<JumpComponent>();
			_jump.SetGroundContactFilter(_playerSettings.MoveSettings.GroundContactFilter);
		}

		private void UpdateCameraReference() {
			_camera = _mainCameraAnchor.Value.GetComponent<Camera>();
		}
	}
}
