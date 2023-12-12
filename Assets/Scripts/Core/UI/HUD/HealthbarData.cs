using UnityEngine;

namespace Frogalypse.UI {
	[CreateAssetMenu(fileName = "HealthbarData", menuName = "Frogalypse/UI/Data/Healthbar")]
	public class HealthbarData : ScriptableObject {
		[field: SerializeField] public Sprite EmptyLeft { get; private set; }
		[field: SerializeField] public Sprite EmptyMid { get; private set; }
		[field: SerializeField] public Sprite EmptyRight { get; private set; }
		[field: SerializeField] public Sprite FilledLeft { get; private set; }
		[field: SerializeField] public Sprite FilledMid { get; private set; }
		[field: SerializeField] public Sprite FilledRight { get; private set; }
	}
}
