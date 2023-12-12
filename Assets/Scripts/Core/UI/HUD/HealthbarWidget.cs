using Frogalypse.Components;

using UnityEngine;
using UnityEngine.UIElements;

namespace Frogalypse.UI {
	public class HealthbarWidget : VisualElement {
		public new class UxmlFactory : UxmlFactory<HealthbarWidget> { }

		private HealthbarData _sprites;
		private HealthComponent _health;

		private byte Current => _health.CurrentHealth;
		private byte Max => _health.MaxHealth.Value;

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

		internal void Redraw() {
			if (_health == null) {
				Debug.LogError($"Tried to draw health bar but health component isn't set correctly", _health);
				return;
			}

			Clear(); // Remove all children

			for (byte i = 1 ; i <= Max ; i++) {
				Image image = new();
				if (i == 1) {
					image.sprite = i <= Current ? _sprites.FilledLeft : _sprites.EmptyLeft;
				} else if (i == Max) {
					image.sprite = i <= Current ? _sprites.FilledRight : _sprites.EmptyRight;
				} else {
					image.sprite = i <= Current ? _sprites.FilledMid : _sprites.EmptyMid;
				}
				Add(image);
			}
		}
	}
}
