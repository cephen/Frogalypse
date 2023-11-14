using Frogalypse.Persistence;

using SideFX.Events;
using SideFX.Scenes;

using UnityEngine.UIElements;

namespace Frogalypse.UI {
	internal sealed class LevelFrame : Button {
		internal new class UxmlFactory : UxmlFactory<LevelFrame> { }

		internal void Init(int levelNumber, GameplaySceneSO scene, LevelRecord data, LoadEventChannelSO channel) {
			ClearClassList();
			AddToClassList("level-frame");
			AddToClassList(data.IsComplete ? "complete-level" : "incomplete-level");

			text = $"{levelNumber}";
			clicked += () => channel.RaiseEvent(scene, showLoadingScreen: true, fadeScreen: true);
		}

		// Custom UI controls need a default constructor
		public LevelFrame() { }
	}
}
