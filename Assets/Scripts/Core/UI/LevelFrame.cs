using Frogalypse.Levels;

using SideFX.SceneManagement;

using UnityEngine.UIElements;

namespace Frogalypse.UI {
	internal sealed class LevelFrame : Button {
		internal new class UxmlFactory : UxmlFactory<LevelFrame> { }

		internal void Init(int levelNumber, LevelData data, LoadEventChannel channel) {
			ClearClassList();
			AddToClassList("level-frame");
			AddToClassList(data.IsCompleted ? "complete-level" : "incomplete-level");

			text = $"{levelNumber}";
			clicked += () => channel.Raise(new LoadRequest {
				SceneData = data.Scene,
				ShowLoadingScreen = true,
				FadeScreen = true
			});
		}

		// Custom UI controls need a default constructor
		public LevelFrame() { }
	}
}
