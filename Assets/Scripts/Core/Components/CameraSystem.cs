using Cinemachine;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	internal sealed class CameraSystem : MonoBehaviour {
		[SerializeField] private Camera _mainCamera;
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		[SerializeField] private TransformAnchor _cameraTransformAnchor;
		[SerializeField] private TransformAnchor _playerTransformAnchor;

		private void OnEnable() {
			_cameraTransformAnchor.Provide(_mainCamera.transform);
			_playerTransformAnchor.OnAnchorUpdated += SetCameraTarget;
		}

		private void OnDisable() {
			_cameraTransformAnchor.Unset();
			_playerTransformAnchor.OnAnchorUpdated -= SetCameraTarget;
		}

		private void Start() {
			SetCameraTarget();
		}

		private void SetCameraTarget() {
			if (!_playerTransformAnchor.IsSet) {
				Debug.LogError("Tried to set camera follow target to player, but the player transform anchor isn't set", _playerTransformAnchor);
				return;
			}
			Transform target = _playerTransformAnchor.Value;
			_virtualCamera.Follow = target;
			_virtualCamera.LookAt = target;
			_virtualCamera.OnTargetObjectWarped(target, target.position - _virtualCamera.transform.position);
		}
	}
}
