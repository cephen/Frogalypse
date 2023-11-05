using UnityEngine;

namespace Frogalypse.Settings {
	// Disabled becauseonly one asset instance is needed
	//[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Frogalypse/Settings/Player")]
	internal class PlayerSettings : ScriptableObject {
		internal ActorMoveSettings MoveSettings;

		internal TetherSettings tetherSettings;

		[Header("Animation")]
		internal AnimationCurve firingSpeedCurve;
	}
}
