using DG.Tweening;

using SideFX.Events;

using UnityEngine;
using UnityEngine.UI;

namespace Frogalypse {
	public class FadeController : MonoBehaviour {
		[SerializeField] private FadeEventChannelSO _fadeChannel;
		[SerializeField] private Image _image;

		private void OnEnable() => _fadeChannel.OnEventRaised += StartFade;
		private void OnDisable() => _fadeChannel.OnEventRaised -= StartFade;

		/// <summary>
		/// Controls the colour of a UI Image in order to fade in or out of gameplay
		/// </summary>
		/// <param name="fadeIn">
		/// If false, the screen becomes black.
		/// If true, the rectangle image fades out, making gameplay visible
		/// </param>
		/// <param name="duration">Time taken in seconds for the fade to complete</param>
		/// <param name="targetColor">The colour to fade to</param>
		private void StartFade(bool fadeIn, float duration, Color targetColor) {
			_image.DOBlendableColor(targetColor, duration);
		}
	}
}