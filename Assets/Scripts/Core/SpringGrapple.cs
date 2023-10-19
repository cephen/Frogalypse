using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Rigidbody2D), typeof(SpringJoint2D))]
	public class SpringGrapple : MonoBehaviour {
		private Rigidbody2D _rb;
		private SpringJoint2D _spring;

		private void Awake() {
			_rb = GetComponent<Rigidbody2D>();
			_spring = GetComponent<SpringJoint2D>();
			ConfigureSpringJoint();
		}

		private void ConfigureSpringJoint() {
			_spring.breakForce = float.MaxValue;
			_spring.breakTorque = float.MaxValue;
			_spring.anchor = Vector2.zero;
			_spring.connectedAnchor = Vector2.zero;
			_spring.enableCollision = false;
			_spring.autoConfigureDistance = false;
			_spring.enabled = false;
		}

		public void ConnectPlayer(Rigidbody2D playerBody) {
			_spring.connectedBody = playerBody;
			_spring.enabled = true;
			_spring.distance = Vector2.Distance(_rb.position, playerBody.position);
		}

		public void Update() => Debug.DrawLine(_rb.position, _spring.connectedBody.position, Color.red);
	}
}
