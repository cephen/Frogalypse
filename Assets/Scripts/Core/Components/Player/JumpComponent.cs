using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(Rigidbody2D))]
	internal sealed class JumpComponent : MonoBehaviour {
		[SerializeField] private float _jumpForce;
		[SerializeField] private float _fallingGravityScale = 1f;

		private Rigidbody2D _body;
		private ContactFilter2D _contactFilter;
		private State _state = State.Grounded;

		private enum State : byte {
			Grounded, Rising, Falling
		}

		private void Awake() => _body = TryGetComponent(out Rigidbody2D component) ? component : gameObject.AddComponent<Rigidbody2D>();

		private void FixedUpdate() {
			// TODO: If the player walks off an edge they aren't switched to the falling state
			switch (_state) {
				case State.Rising:
					if (_body.velocity.y < 0f) {
						_state = State.Falling;
					}
					break;
				case State.Falling:
					_body.gravityScale = _fallingGravityScale;
					break;
				default:
					break;
			}
		}

		internal void OnJump() {
			if (!IsGrounded()) {
				return;
			}
			_body.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
			_state = State.Rising;
		}

		internal void OnJumpCancelled() {
			_state = State.Falling;
		}

		internal void SetGroundContactFilter(ContactFilter2D filter) => _contactFilter = filter;

		private void OnCollisionEnter2D() {
			if (IsGrounded()) {
				_state = State.Grounded;
				_body.gravityScale = 1f;
			}
		}

		private bool IsGrounded() {
			// The length of the array determines the number of hits to gather
			// Since only colliders that pass through the contact filter will be counted,
			// only one collider needs to be found to consider the actor grounded.
			// TODO: Current logic will ground the player when they touch terrain above them, this might be unwanted behaviour
			Collider2D[] hits = new Collider2D[1];
			return _body.GetContacts(_contactFilter, hits) > 0;
		}
	}
}
