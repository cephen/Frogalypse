using Frogalypse.Levels;

using SideFX.SceneManagement;

using UnityEngine.UIElements;

namespace Frogalypse.UI {
	internal sealed class LevelFrame : Button {
		internal new class UxmlFactory : UxmlFactory<LevelFrame> { }

		internal void Init(int levelNumber, GameplayScene scene, LevelRecord data, LoadEventChannel channel) {
			ClearClassList();
			AddToClassList("level-frame");
			AddToClassList(data.IsComplete ? "complete-level" : "incomplete-level");

			text = $"{levelNumber}";
			clicked += () => channel.Raise(new LoadRequest(scene));
		}

		// Custom UI controls need a default constructor
		public LevelFrame() { }
	}
}
