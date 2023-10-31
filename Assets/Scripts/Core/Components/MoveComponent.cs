using Frogalypse.Settings;

using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(Rigidbody2D))]
	public class MoveComponent : MonoBehaviour {
		[SerializeField] private ActorMoveSettings _settings;

		private Rigidbody2D _body;
		private Vector2 _input = Vector2.zero;

		private void Awake() => _body = TryGetComponent(out Rigidbody2D component) ? component : gameObject.AddComponent<Rigidbody2D>();

		private void FixedUpdate() {
			if (_settings == null) {
				Debug.Log("Actor Move Settings not provided D:", _settings);
				enabled = false;
				return;
			}

			// Check Grounded State
			// true: Horizontal input only, use walk settings
			// false: All input, use air control settings

			if (IsGrounded()) {
				HandleGroundedMovement();
			} else if (_settings.CanAirControl) {
				HandleAirControl();
			}
		}

		/// <summary>
		/// Set the travel direction of this actor
		/// </summary>
		public void ProvideInput(Vector2 input) => _input = input;

		/// <summary>
		/// Change the settings used by this movement component.
		/// Typically only called when initialising the component.
		/// </summary>
		public void ProvideSettings(ActorMoveSettings settings) => _settings = settings;

		private bool IsGrounded() {
			// The length of the array determines the number of hits to gather
			// Since only colliders that pass through the contact filter will be counted,
			// only one collider needs to be found to consider the actor grounded.
			Collider2D[] hits = new Collider2D[1];
			return _body.GetContacts(_settings.GroundContactFilter, hits) > 0;
		}

		private void HandleGroundedMovement() {
			Vector2 velocity = _body.velocity;

			if (Mathf.Abs(_input.x) > Mathf.Epsilon) {
				// Some movement input, accelerate player
				float input = Mathf.Clamp(_input.x, -1f, 1f);
				float inputDirection = Mathf.Sign(input); // returns (-1: Left) or (1: Right)
				float travelDirection = Mathf.Sign(velocity.x);

				// If input direction is opposite of travel direction, use acceleration + deceleration
				float accelScale = travelDirection == inputDirection ? _settings.WalkAcceleration : _settings.WalkAcceleration + _settings.WalkDeceleration;

				velocity.x = Mathf.MoveTowards(velocity.x, _settings.MaxWalkSpeed * inputDirection, accelScale * Time.fixedDeltaTime);
			} else {
				// No move input, decelerate player
				velocity.x = Mathf.MoveTowards(velocity.x, 0f, _settings.WalkDeceleration * Time.fixedDeltaTime);
			}

			_body.velocity = velocity;
		}

		private void HandleAirControl() {
			Vector2 velocity = _body.velocity;
			float horizontal = Mathf.Clamp(_input.x, -1f, 1f);
			float vertical = Mathf.Clamp(_input.y, -1f, 1f);

			if (Mathf.Abs(horizontal) > Mathf.Epsilon) {
				velocity.x += horizontal * _settings.AirControlSideAcceleration * Time.fixedDeltaTime;
			} else {
				// No horizontal input, decelerate on this axis
				velocity.x = Mathf.MoveTowards(velocity.x, 0f, _settings.AirControlDeceleration * Time.fixedDeltaTime);
			}

			if (Mathf.Abs(vertical) > Mathf.Epsilon) {
				// Which direction is the input? -1 = down, 1 = up
				float sign = Mathf.Sign(vertical);
				float magnitude = Mathf.Abs(vertical);
				float acceleration = sign > 0f ? _settings.AirControlUpAcceleration : _settings.AirControlDownAcceleration;
				velocity.y += sign * magnitude * acceleration * Time.fixedDeltaTime;
			}

			_body.velocity = velocity;
		}

	}
}
