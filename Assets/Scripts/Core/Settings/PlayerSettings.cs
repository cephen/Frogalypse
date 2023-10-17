using UnityEngine;

namespace Frogalypse.Settings {
	[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Frogalypse/Settings/Player")]
	public class PlayerSettings : ScriptableObject {
		public float grappleReach;
	}

}
