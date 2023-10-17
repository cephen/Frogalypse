﻿using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Frogalypse.Input {
	/// <summary>
	/// InputReader provides a single source of input events to all other game systems using C# Events
	/// The GameInput class is generated by Unity's InputSystem package, and defines interfaces that must be implemented to 
	/// </summary>
	[CreateAssetMenu(fileName = "Input Reader", menuName = "Frogalypse/Input Reader")]
	public class InputReader : ScriptableObject, GameInput.IGameplayActions {
		/// <summary>
		/// Invoked whenever the player moves the mouse.
		/// Event data is the screen position of the mouse.
		/// </summary>
		public event Action<Vector2> AimEvent = delegate { };

		/// <summary>
		/// Property that stores the screen position of the mouse.
		/// For components that don't need live updates.
		/// </summary>
		public Vector2 MousePosition { get; private set; }

		/// <summary>
		/// Invoked when the player presses a movement button
		/// -1 => move left
		/// +1 => move right
		/// </summary>
		public event Action<float> MoveEvent = delegate { };

		/// <summary>
		/// Invoked when the player presses the grapple button
		/// </summary>
		public event Action GrappleEvent = delegate { };

		/// <summary>
		/// Invoked when the player releases the grapple button
		/// </summary>
		public event Action GrappleCancelledEvent = delegate { };

		/// <summary>
		/// Invoked when the player presses the jump button
		/// </summary>
		public event Action JumpEvent = delegate { };

		/// <summary>
		/// Invoked when the player releases the jump button
		/// </summary>
		public event Action JumpCancelledEvent = delegate { };

		#region Setup

		private GameInput _inputSource;

		private void OnEnable() {
			if ( _inputSource == null ) {
				_inputSource = new();
				_inputSource.Gameplay.SetCallbacks(this);
			}
			_inputSource.Gameplay.Enable();
		}

		private void OnDisable() => _inputSource?.Gameplay.Disable();

		#endregion

		#region Handlers

		public void OnAim(InputAction.CallbackContext context) {
			if ( context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Started ) {
				var aim = context.ReadValue<Vector2>();
				MousePosition = aim;
				AimEvent?.Invoke(aim);
				//Debug.Log($"Aiming at ({aim.x}, {aim.y})");
			}
		}

		public void OnGrapple(InputAction.CallbackContext context) {
			switch ( context.phase ) {
				case InputActionPhase.Performed:
					Debug.Log("Grapple Started");
					GrappleEvent?.Invoke();
					return;
				case InputActionPhase.Canceled:
					Debug.Log("Grapple Cancelled");
					GrappleCancelledEvent?.Invoke();
					return;
			}
		}

		public void OnJump(InputAction.CallbackContext context) {
			switch ( context.phase ) {
				case InputActionPhase.Performed:
					Debug.Log("Jump Started");
					JumpEvent?.Invoke();
					return;
				case InputActionPhase.Canceled:
					Debug.Log("Jump Cancelled");
					JumpCancelledEvent?.Invoke();
					return;
			}
		}

		public void OnMove(InputAction.CallbackContext context) {
			switch ( context.phase ) {
				case InputActionPhase.Performed:
					MoveEvent?.Invoke(context.ReadValue<float>());
					return;
				case InputActionPhase.Canceled:
					MoveEvent?.Invoke(0f);
					return;
			}
		}

		#endregion
	}
}
