using UnityEngine;

namespace SideFX {
	public class DescriptionBaseSO : ScriptableObject {
		[SerializeField, TextArea(3, 6)] private string _description;
	}
}
