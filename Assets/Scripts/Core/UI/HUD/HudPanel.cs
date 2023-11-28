using UnityEngine;
using UnityEngine.UIElements;

namespace Frogalypse.UI {
	[RequireComponent(typeof(UIDocument))]
	public class HudPanel : MonoBehaviour {
		[SerializeField] private HealthbarData _healthbarData;

		private UIDocument _document;
		private VisualElement _root;
		private HealthbarWidget _healthBar;

		private void Awake() {
			// TODO: Handle nulls
			if (TryGetComponent(out UIDocument component)) {
				_document = component;
				_root = _document.rootVisualElement;
				_healthBar = _root.Q<HealthbarWidget>("healthbar");
			}

			if (_healthBar != null && _healthbarData != null)
				_healthBar.Init(_healthbarData);
		}
	}
}
