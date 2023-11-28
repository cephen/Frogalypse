using System;

using UnityEngine;

namespace Frogalypse.Settings {
	[Serializable]
	internal struct TetherSettings {
		public float TargetLength;
		public float TravelTime;
		public float MaxTravelDistance;
		public ContactFilter2D ContactFilter;
	}
}
