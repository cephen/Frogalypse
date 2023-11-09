using System.Collections.Generic;

using UnityEngine;

namespace Frogalypse.Levels {
	[CreateAssetMenu(fileName = "LevelDB", menuName = "Frogalypse/Databases/Levels")]
	internal class LevelDB : ScriptableObject {
		[SerializeField] private List<LevelData> _levels;

		public LevelData this[int i] => _levels[i];

		public int Count => _levels.Count;
	}
}
