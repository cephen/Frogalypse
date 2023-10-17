using System;

using UnityEngine;

namespace Frogalypse {
	[RequireComponent(typeof(Rigidbody2D), typeof(HingeJoint2D))]
	public class HingeGrapple : MonoBehaviour {
		private Rigidbody2D _rb;
		private HingeJoint2D _joint;

		private void Awake() {
			_rb = GetComponent<Rigidbody2D>();
			_joint = GetComponent<HingeJoint2D>();
			ConfigureJoint();
		}

		private void ConfigureJoint() {
			_joint.breakForce = float.MaxValue;
			_joint.breakTorque = float.MaxValue;
			_joint.enableCollision = false;
			_joint.useMotor = true;
			_joint.motor = new JointMotor2D() { motorSpeed = 20f, maxMotorTorque = float.MaxValue };
			_joint.limits = new() { max = 360f, min = 0f };
		}

		public void ConnectPlayer(Rigidbody2D playerBody) {
			_joint.connectedBody = playerBody;
			_joint.enabled = true;
			//_joint.distance = Vector2.Distance(_rb.position, playerBody.position);
		}

		public void Update() {
			Debug.DrawLine(_rb.position, _joint.connectedBody.position, Color.red);
		}
	}
}
