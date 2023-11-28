using UnityEngine;
using UnityEngine.UIElements;

namespace Frogalypse.UI {
	[RequireComponent(typeof(UIDocument))]
	public class HudPanel : MonoBehaviour {
		private UIDocument _document;
		private VisualElement _root;
		private HealthbarWidget _healthBar;

		private void Awake() {
			if (TryGetComponent(out UIDocument component)) {
				_document = component;
				_root = _document.rootVisualElement;
				_healthBar = _root.Q<HealthbarWidget>("health-bar");
			}
		}
	}
}
