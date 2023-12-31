using UnityEngine;

namespace Frogalypse.Settings {
	// Disabled becauseonly one asset instance is needed
	//[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Frogalypse/Settings/Player")]
	internal class PlayerSettings : ScriptableObject {
		[SerializeField] internal ActorMoveSettings MoveSettings;

		[SerializeField] internal TetherSettings TetherSettings;

		[Header("Animation")]
		[SerializeField] internal AnimationCurve FiringSpeedCurve;
	}
}
