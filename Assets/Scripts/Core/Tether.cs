using System.Collections;

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
		[SerializeField] private TransformAnchor _tetherStartPositionAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;

		// Components
		private Line _line;

		// Fields
		private Vector3 _targetPosition = Vector3.zero;
		[SerializeField] private TetherState _state = TetherState.Disabled;

		private enum TetherState : byte {
			Ready = 0,
			Firing = 1,
			Attached = 2,
			Reeling = 3,
			Disabled = 4,
		}

		private void Awake() {
			_line = GetComponent<Line>();
			_line.enabled = false;
		}

		private void OnEnable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				Destroy(gameObject);
			}
			_input.GrappleEvent += CreateTether;
			_input.GrappleCancelledEvent += ReloadTether;
		}

		private void OnDisable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				Destroy(gameObject);
			}
			_input.GrappleEvent -= CreateTether;
		}

		private void Update() {
			if (_tetherStartPositionAnchor.IsSet) {
				_line.Start = _tetherStartPositionAnchor.Value.position;
			} else {
				_line.enabled = false;
			}
		}

		private void CreateTether() => StartCoroutine(FireTether());

		private void ReloadTether() {
			_line.enabled = false;
			_state = TetherState.Reeling;
			return;
		}

		private IEnumerator FireTether() {
			_state = TetherState.Firing;
			_line.enabled = true;

			Vector3 reticle = _reticleAnchor.Value.position;


			for (float i = 0f ; i < _playerSettings.timeToHitTarget ; i += Time.deltaTime) {
				float t = Mathf.Lerp(0f, Vector3.Distance(_tetherStartPositionAnchor.Value.position, reticle), i / _playerSettings.timeToHitTarget);
				_line.End = Vector3.Lerp(_tetherStartPositionAnchor.Value.position, reticle, t);
				yield return null;
			}

			// TODO: implement hitting an object
			// TODO: implement missing an object
			// TODO: implement hitting the max distance

		}
	}
}
