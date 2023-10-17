using System.Collections;
using System.Collections.Generic;

using Frogalypse.Input;

using UnityEngine;

namespace Frogalypse {
	public class PlayerController : MonoBehaviour {
		[SerializeField] private InputReader _input;

		private void Awake() {
			if ( _input == null ) {
				Debug.LogError($"Input reader isn't set!!");
			}
		}
	}
}
