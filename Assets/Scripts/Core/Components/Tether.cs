using System.Collections;

using Frogalypse.Input;
using Frogalypse.Settings;

using Shapes;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Line), typeof(SpringJoint2D))]
	public class Tether : MonoBehaviour {
		[Header("Assets")]
		[SerializeField] private InputReader _input;
		[SerializeField] private PlayerSettings _playerSettings;
		[SerializeField] private TransformAnchor _tetherStartPositionAnchor;
		[SerializeField] private TransformAnchor _reticleAnchor;

		// Components
		private Line _line;
		private SpringJoint2D _spring;
		private Rigidbody2D _body;

		// Fields
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
			_spring = GetComponent<SpringJoint2D>();
			_body = GetComponent<Rigidbody2D>();
			_line.enabled = false;
			_spring.enabled = false;
		}

		private void OnEnable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				return;
			}
			_input.TetherEvent += OnTether;
			_input.TetherCancelledEvent += OnTetherCancelled;
		}

		private void OnDisable() {
			if (_input == null) {
				Debug.LogError("Input Reader isn't set", _input);
				return;
			}
			_input.TetherEvent -= OnTether;
			_input.TetherCancelledEvent -= OnTetherCancelled;
		}

		private void Update() {
			if (_tetherStartPositionAnchor.IsSet) {
				_line.Start = _tetherStartPositionAnchor.Value.position;
			} else {
				_line.enabled = false;
			}
		}

		private void OnTether() {
			if (_state is TetherState.Ready)
				StartCoroutine(FireTether());

		}

		private void OnTetherCancelled() {
			_line.enabled = false;
			_state = TetherState.Reeling;

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
