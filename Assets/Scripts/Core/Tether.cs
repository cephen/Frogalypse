using Frogalypse.Input;

using SideFX;

using UnityEngine;

namespace Frogalypse {
	public class Tether : MonoBehaviour {
		[SerializeField] private TransformAnchor _playerAnchor;
		[SerializeField] private InputReader _input;

		// Components

		private void Awake() => _lineRenderer = GetComponent<LineRenderer>();

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

		private void CreateTether() { }
	}
}
