using UnityEngine;

namespace Frogalypse.Settings {
	[CreateAssetMenu(fileName = "ActorMoveSettings", menuName = "Frogalypse/Settings/Actors/Movement")]
	public class ActorMoveSettings : ScriptableObject {
		[Header("Walking")]
		public float MaxWalkSpeed;
		public float WalkAcceleration;
		public float WalkDeceleration;

		[Header("Air Control Settings")]
		public bool CanAirControl = false;
		public float AirControlUpAcceleration;
		public float AirControlDownAcceleration;
		public float AirControlSideAcceleration;
		public float AirControlDeceleration;

		[Header("Grounding Settings")]
		public bool UseGravity;
		public ContactFilter2D GroundContactFilter;
	}
}
