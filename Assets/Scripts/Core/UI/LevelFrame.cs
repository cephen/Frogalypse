using Frogalypse.Levels;

using SideFX.Events;

using UnityEngine.UIElements;

namespace Frogalypse.UI {
	internal sealed class LevelFrame : Button {
		internal new class UxmlFactory : UxmlFactory<LevelFrame> { }

		internal void Init(int levelNumber, LevelData data, LoadEventChannelSO channel) {
			ClearClassList();
			AddToClassList("level-frame");
			AddToClassList(data.IsCompleted ? "complete-level" : "incomplete-level");

			text = $"{levelNumber}";
			clicked += () => channel.RaiseEvent(data.Scene, showLoadingScreen: true, fadeScreen: true);
		}

		// Custom UI controls need a default constructor
		public LevelFrame() { }
	}
}
