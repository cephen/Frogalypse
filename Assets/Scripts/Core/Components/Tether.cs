using Frogalypse.Components;
using Frogalypse.Input;
using Frogalypse.Settings;

using Shapes;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Line))]
	public class Tether : MonoBehaviour {
		[Header("Assets")]
		[SerializeField] private InputReader _input;
		[SerializeField] private PlayerSettings _playerSettings;
		[SerializeField] private TransformAnchor _hookLauncherAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;

		// Components
		private Line _line;
		private GrapplingHook _hook;

		// Fields
		[SerializeField] private TetherState _state = TetherState.Ready;

		private enum TetherState : byte {
			Ready,
			Firing,
			Attached,
		}

		private void Awake() {
			_hook = transform.GetChild(0).GetComponent<GrapplingHook>();
			_hook.UpdateSettings(_playerSettings.tetherSettings);
			_line = GetComponent<Line>();
		}

		private void OnEnable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				return;
			}
			_input.TetherEvent += OnLaunchTether;
			_input.TetherCancelledEvent += OnTetherCancelled;
			_hook.HitEvent += OnHookHit;
			_hook.MissEvent += OnHookMiss;
			_line.enabled = false;
		}

		private void OnDisable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				return;
			}
			_input.TetherEvent -= OnLaunchTether;
			_input.TetherCancelledEvent -= OnTetherCancelled;
			_hook.HitEvent -= OnHookHit;
			_hook.MissEvent -= OnHookMiss;
			_line.enabled = false;
		}

		private void Update() {
			if (!_hookLauncherAnchor.IsSet) {
				Debug.LogError("Hook Launcher Anchor isn't set", _hookLauncherAnchor);
				enabled = false;
			}
			switch (_state) {
				case TetherState.Ready:
					_line.enabled = false;
					_line.End = Vector3.zero;
					break;
				case TetherState.Firing:
					_line.enabled = true;
					_line.End = _hook.transform.localPosition;
					break;
				case TetherState.Attached:
					_line.enabled = true;
					_line.End = _hook.transform.localPosition;
					break;
			}
			transform.position = _hookLauncherAnchor.Value.position;
		}

		private void OnTether() {
			if (_state is not TetherState.Ready) {
				return;
			}
			_hook.Fire();
			_state = TetherState.Firing;
		}

		private void OnTetherCancelled() {
			_state = TetherState.Ready;
			_hook.Cancel();
		}

		private void OnHookHit() => _state = TetherState.Attached;
		private void OnHookMiss() => _state = TetherState.Ready;
	}
}
