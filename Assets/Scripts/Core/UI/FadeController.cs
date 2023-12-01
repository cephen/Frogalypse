using DG.Tweening;

using SideFX.Events;
using SideFX.SceneManagement.Events;

using UnityEngine;
using UnityEngine.UI;

namespace Frogalypse {
	/// <summary>
	/// Controls the colour of a UI Image in order to fade in or out of gameplay
	/// </summary>
	public class FadeController : MonoBehaviour {
		[SerializeField] private Image _image;

		private EventBinding<FadeIn> _fadeInBinding;
		private EventBinding<FadeTo> _fadeToBinding;

		private void OnEnable() {
			_fadeInBinding = new EventBinding<FadeIn>(OnFadeIn);
			_fadeToBinding = new EventBinding<FadeTo>(OnFadeTo);
			EventBus<FadeIn>.Register(_fadeInBinding);
			EventBus<FadeTo>.Register(_fadeToBinding);
		}

		private void OnDisable() {
			EventBus<FadeIn>.Deregister(_fadeInBinding);
			EventBus<FadeTo>.Deregister(_fadeToBinding);
		}

		private void OnFadeIn(FadeIn fade) => _image.DOBlendableColor(Color.clear, fade.Duration);
		private void OnFadeTo(FadeTo fade) => _image.DOBlendableColor(fade.TargetColor, fade.Duration);
	}
}
