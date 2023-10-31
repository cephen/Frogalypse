using UnityEngine;

namespace Frogalypse.Settings {
	// Disabled becauseonly one asset instance is needed
	//[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Frogalypse/Settings/Player")]
	public class PlayerSettings : ScriptableObject {
		public ActorMoveSettings MoveSettings;

		[Header("Tether")]
		public float maxGrappleDistance;
		public ContactFilter2D grappleContactFilter;

		[Header("Animation")]
		public float timeToHitTarget = 0.5f;
		public float reelingSpeed = 1f / 3;
		public AnimationCurve firingSpeedCurve;
		public AnimationCurve reelingSpeedCurve;
	}
}
