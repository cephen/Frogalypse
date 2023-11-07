using UnityEngine;
using UnityEngine.UIElements;

namespace Frogalypse.UI {
	public class LevelFrame : Button {
		public new class UxmlFactory : UxmlFactory<LevelFrame> { }
		public void Init(int levelNumber, bool complete) {
			style.backgroundColor = complete ? Color.white : Color.clear;
			text = $"{levelNumber:00}";
		}

		// Custom UI controls need a default constructor
		public LevelFrame() {
			AddToClassList("level-frame");
			Init(0, false);
		}
	}
}
