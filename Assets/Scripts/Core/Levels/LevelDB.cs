using UnityEngine;

namespace Frogalypse.Levels {
	[CreateAssetMenu(fileName = "LevelDB", menuName = "Frogalypse/Databases/Levels")]
	internal class LevelDB : ScriptableObject {
		[SerializeField] private LevelData[] _levels;

		public LevelData this[int i] => _levels[i];

		public int Count => _levels.Length;
	}
}
