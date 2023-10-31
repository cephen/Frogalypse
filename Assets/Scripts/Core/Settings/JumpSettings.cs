using UnityEngine;

namespace Frogalypse.Settings {
	[CreateAssetMenu(fileName = "ActorJumpSettings", menuName = "Frogalypse/Settings/Actors/Jump")]
	public class JumpSettings : MonoBehaviour {
		public float JumpForce;
		public float MaxJumpHoldTime;
		[Range(0f, 1f)] public float MinGravityScale;
	}
}
