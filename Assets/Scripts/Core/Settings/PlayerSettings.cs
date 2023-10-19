using UnityEngine;

namespace Frogalypse.Settings {
	// Disabled becauseonly one asset instance is needed
	//[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Frogalypse/Settings/Player")]
	public class PlayerSettings : ScriptableObject {
		[Header("Movement")]
		public float groundMoveSpeed = 2f;
		public float airMoveSpeed = 3f;
		public float jumpForce = 5f;

		[Header("Tether")]
		public float maxGrappleDistance;
		public ContactFilter2D grappleContactFilter;

		[Header("Animation")]
		public float firingSpeed = 5f;
		public float reelingSpeed = 4f;
		public AnimationCurve firingSpeedCurve;
		public AnimationCurve reelingSpeedCurve;
	}
}
