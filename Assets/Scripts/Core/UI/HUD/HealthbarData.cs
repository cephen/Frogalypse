using Frogalypse.SharedData;

using UnityEngine;

namespace Frogalypse.UI {
	[CreateAssetMenu(fileName = "HealthbarData", menuName = "Frogalypse/UI/Data/Healthbar")]
	public class HealthbarData : ScriptableObject {
		[SerializeField] private Sprite _emptyLeft;
		[SerializeField] private Sprite _emptyMid;
		[SerializeField] private Sprite _emptyRight;
		[SerializeField] private Sprite _filledLeft;
		[SerializeField] private Sprite _filledMid;
		[SerializeField] private Sprite _filledRight;
		[SerializeField] private HealthData _playerHealthData;

		public Sprite EmptyLeft => _emptyLeft;
		public Sprite EmptyMid => _emptyMid;
		public Sprite EmptyRight => _emptyRight;
		public Sprite FilledLeft => _filledLeft;
		public Sprite FilledMid => _filledMid;
		public Sprite FilledRight => _filledRight;
		public HealthData PlayerHealth => _playerHealthData;
	}
}
