using System;
using Frogalypse.Input;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	public class Tether : MonoBehaviour {
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private InputReader _input;

		// Components
		// Fields
		private Vector3 _targetPosition = Vector3.zero;
		[SerializeField] private TetherState _state = TetherState.Disabled;

		private void Awake() => _lineRenderer = GetComponent<LineRenderer>();
		private enum TetherState : byte {
			Ready = 0,
			Firing = 1,
			Attached = 2,
			Reeling = 3,
			Disabled = 4,
		}

		private void OnEnable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				Destroy(gameObject);
			}
			_input.GrappleEvent += CreateTether;
		}

		private void OnDisable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				Destroy(gameObject);
			}
			_input.GrappleEvent -= CreateTether;
		}

		private void CreateTether() {
			switch (_state) {
				case TetherState.Ready:
					Debug.Log("Tether is ready, firing!");
					_state = TetherState.Firing;
					return;
				case TetherState.Firing:
					Debug.LogWarning($"Tether already launched, can't launch until reeled in", this);
					return;
				case TetherState.Attached:
					Debug.Log("Already tethered to an object, ignoring CreateTether", this);
					return;
				case TetherState.Reeling:
					Debug.Log("Tether is being reeled in, can't create new tether just yet");
					return;
				case TetherState.Disabled:
					Debug.LogWarning("Failed to create tether: tether is disabled", this);
					return;
				default:
					throw new ArgumentOutOfRangeException(nameof(_state), _state, "Tether state value not recognised");
			};
		}
	}
}
