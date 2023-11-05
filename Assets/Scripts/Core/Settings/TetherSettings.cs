using System;

using UnityEngine;

namespace Frogalypse.Settings {
	[Serializable]
	internal struct TetherSettings {
		public float targetLength;
		public float travelTime;
		public float maxTravelDistance;
		public ContactFilter2D contactFilter;
	}
}
