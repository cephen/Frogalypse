using SideFX.Scenes;

using UnityEngine;

namespace Frogalypse.Levels {
	[CreateAssetMenu(fileName = "Level x", menuName = "Frogalypse/Level Data")]
	internal class LevelData : ScriptableObject {
		[SerializeField] private GameplaySceneSO _scene;
		[SerializeField] private bool _isCompleted = false;

		internal GameplaySceneSO Scene => _scene;
		internal bool IsCompleted => _isCompleted;

		internal void MarkComplete() => _isCompleted = true;
	}
}
