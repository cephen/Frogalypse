using Frogalypse.Settings;

using UnityEngine;

namespace Frogalypse.Components {
	[RequireComponent(typeof(Rigidbody2D))]
	public class JumpComponent : MonoBehaviour {
		[SerializeField] private JumpSettings _settings;

		private Rigidbody2D _body;
		private ContactFilter2D _contactFilter;

		private void Awake() => _body = TryGetComponent(out Rigidbody2D component) ? component : gameObject.AddComponent<Rigidbody2D>();

		public void OnJump() {
			if (!IsGrounded()) {
				return;
			}
			_body.AddForce(Vector2.up * _settings.JumpForce, ForceMode2D.Impulse);
		}

		public void ProvideSettings(JumpSettings settings) => _settings = settings;
		public void SetGroundContactFilter(ContactFilter2D filter) => _contactFilter = filter;

		private bool IsGrounded() {
			// The length of the array determines the number of hits to gather
			// Since only colliders that pass through the contact filter will be counted,
			// only one collider needs to be found to consider the actor grounded.
			Collider2D[] hits = new Collider2D[1];
			return _body.GetContacts(_contactFilter, hits) > 0;
		}
	}
}
