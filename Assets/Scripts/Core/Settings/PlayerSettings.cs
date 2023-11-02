using UnityEngine;

namespace Frogalypse.Settings {
	// Disabled becauseonly one asset instance is needed
	//[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Frogalypse/Settings/Player")]
	public class PlayerSettings : ScriptableObject {
		public ActorMoveSettings MoveSettings;

		[Header("Tether")]
		public TetherSettings tetherSettings;

		[Header("Animation")]
		public AnimationCurve firingSpeedCurve;
	}

	[System.Serializable]
	public struct TetherSettings {
		public float maxLength;
		public float targetLength;
		public float travelTime;
		public ContactFilter2D contactFilter;
	}
}
