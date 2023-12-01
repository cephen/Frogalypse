using Frogalypse.Levels;

using SideFX.Events;
using SideFX.SceneManagement;
using SideFX.SceneManagement.Events;

using UnityEngine.UIElements;

namespace Frogalypse.UI {
	internal sealed class LevelFrame : Button {
		internal new class UxmlFactory : UxmlFactory<LevelFrame> { }

		internal void Init(int levelNumber, GameplayScene scene, LevelRecord data) {
			ClearClassList();
			AddToClassList("level-frame");
			AddToClassList(data.IsComplete ? "complete-level" : "incomplete-level");

			text = $"{levelNumber}";
			clicked += () => EventBus<LoadRequest>.Raise(new LoadRequest(scene));
		}

		// Custom UI controls need a default constructor
		public LevelFrame() { }
	}
}
