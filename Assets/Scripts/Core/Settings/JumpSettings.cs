using UnityEngine;

namespace Frogalypse.Settings {
	[CreateAssetMenu(fileName = "ActorJumpSettings", menuName = "Frogalypse/Actors/Jump Settings")]
	public class JumpSettings : ScriptableObject {
		public float JumpForce;
		public float MaxJumpHoldTime;
		[Range(0f, 1f)] public float MinGravityScale;
	}
}
