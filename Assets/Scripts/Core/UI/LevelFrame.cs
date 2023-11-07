using UnityEngine.UIElements;

namespace Frogalypse.UI {
	public class LevelFrame : Button {
		public new class UxmlFactory : UxmlFactory<LevelFrame> { }
		public void Init(int levelNumber, bool complete) {
			ClearClassList();
			AddToClassList("level-frame");
			AddToClassList(complete ? "complete-level" : "incomplete-level");
			text = $"{levelNumber:00}";
		}

		// Custom UI controls need a default constructor
		public LevelFrame() => Init(0, false);
	}
}
