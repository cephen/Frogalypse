using Frogalypse.Components;
using Frogalypse.Input;
using Frogalypse.Settings;

using SideFX.Anchors;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(MoveComponent), typeof(JumpComponent), typeof(HealthComponent))]
	internal class PlayerController : MonoBehaviour {
		[Header("Assets")]
		[SerializeField] private InputReader _input;
		[SerializeField] private PlayerSettings _playerSettings;
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private TransformAnchor _tetherStartPointAnchor;
		[SerializeField] private TransformAnchor _mainCameraAnchor;

		[Header("Child Transforms")]
		[SerializeField] private Transform _tetherLauncherPivot;
		[SerializeField] private Transform _tetherLauncher;
		[SerializeField] private Transform _tetherStartPoint;

		[SerializeField] private FrogTongue _tongue;

		// Components
		private MoveComponent _mover;
		private JumpComponent _jump;
		private Camera _camera;

		private void Awake() {
			if (_input == null) {
				Debug.LogError($"Input Reader isn't set D:", _input);
				return;
			}

			_tetherStartPointAnchor.Provide(_tetherStartPoint);
			_playerAnchor.Provide(transform);
			if (_tongue != null)
				_tongue.UpdateSettings(_playerSettings.TetherSettings);
			InitMoveComponent();
			InitJumpComponent();
		}

		private void Start() {
			if (_mainCameraAnchor.IsSet)
				UpdateCameraReference();
		}

		private void Update() {
			SetLauncherPivotAngle();
		}

		private void OnEnable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set D:", _input);
				return;
			}
			_input.MoveEvent += _mover.SetInput;
			_input.JumpEvent += _jump.OnJump;
			_input.JumpCancelledEvent += _jump.OnJumpCancelled;
			_mainCameraAnchor.OnAnchorUpdated += UpdateCameraReference;
		}

		private void OnDisable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set D:", _input);
				return;
			}
			_input.MoveEvent -= _mover.SetInput;
			_input.JumpEvent -= _jump.OnJump;
			_input.JumpCancelledEvent -= _jump.OnJumpCancelled;
			_mainCameraAnchor.OnAnchorUpdated -= UpdateCameraReference;
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

		private void SetLauncherPivotAngle() {
			Vector2 target = (Vector2) _camera.ScreenToWorldPoint(_input.MousePosition);
			Vector2 delta = target - (Vector2) transform.position;
			float angle = Vector2.SignedAngle(Vector3.up, delta);
			_tetherLauncherPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}
