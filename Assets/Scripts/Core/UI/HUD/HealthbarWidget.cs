using UnityEngine;
using UnityEngine.UIElements;

namespace Frogalypse.UI {
	public class HealthbarWidget : VisualElement {
		public new class UxmlFactory : UxmlFactory<HealthbarWidget> { }

		private HealthbarData _sprites;
		private HealthComponent _health;

		public HealthbarWidget() { }

		internal void SetSprites(HealthbarData sprites) {
			_sprites = sprites;
			Redraw();
		}

		internal void SetHealthComponent(HealthComponent health) {
			// Unsubscribe if already subscribed (for instance if Init is called twice)
			if (_health != null)
				_health.HealthChanged -= Redraw;

			_health = health;
			_health.HealthChanged += Redraw;

			Redraw();
		}

		public void Redraw() {
			Clear(); // Remove all children
			byte max = _data.PlayerHealth.Max;
			byte current = _data.PlayerHealth.Current;

			for (byte i = 1 ; i <= max ; i++) {
				Image image = new();
				if (i == 1) {
					image.sprite = i <= current ? _data.FilledLeft : _data.EmptyLeft;
				} else if (i == max) {
					image.sprite = i <= current ? _data.FilledRight : _data.EmptyRight;
				} else {
					image.sprite = i <= current ? _data.FilledMid : _data.EmptyMid;
				}
				Add(image);
			}
		}
	}
}
