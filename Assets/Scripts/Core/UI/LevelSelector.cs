using DG.Tweening;

using Frogalypse.Levels;
using Frogalypse.Persistence;

using SideFX.Events;
using SideFX.Scenes;

using UnityEngine.UIElements;

namespace Frogalypse.UI {
	internal sealed class LevelSelector : VisualElement {
		internal new class UxmlFactory : UxmlFactory<LevelSelector> { }

		public LevelSelector() {
			name = "level-selector";
			style.opacity = 0f;
		}

		internal void Init(LoadEventChannelSO channel, LevelDB levelDatabase) {
			Clear(); // Remove existing level frames before adding new ones
			for (int i = 0 ; i < levelDatabase.Count ; i++) {
				GameplaySceneSO scene = levelDatabase[i];
				LevelRecord record = levelDatabase[scene];
				LevelFrame frame = new LevelFrame();
				frame.Init(i + 1, scene, record, channel);
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
