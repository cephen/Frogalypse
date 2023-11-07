using DG.Tweening;

using Frogalypse.Levels;

using UnityEngine.UIElements;

namespace Frogalypse.UI {
	public class LevelSelector : VisualElement {
		public new class UxmlFactory : UxmlFactory<LevelSelector> { }

		public LevelSelector() {
			name = "level-selector";
			style.opacity = 0f;
		}

		internal void Populate(LevelDB levels) {
			for (int i = 0 ; i < levels.Count ; i++) {
				LevelData data = levels[i];
				LevelFrame frame = new();
				frame.Init(i + 1, data.IsCompleted);
				Add(frame);
			}
		}

		internal void Show() =>
			DOTween.To(
				getter: () => style.opacity.value,
				setter: (float x) => style.opacity = x,
				endValue: 1f,
				duration: 1f);

		internal void Hide() =>
			DOTween.To(
				getter: () => style.opacity.value,
				setter: (float x) => style.opacity = x,
				endValue: 0f,
				duration: 1f);

	}
}
