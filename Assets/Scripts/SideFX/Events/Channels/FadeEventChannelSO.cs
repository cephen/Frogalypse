using UnityEngine;

namespace SideFX.Events {
	[CreateAssetMenu(fileName = "Fade Event Channel", menuName = "SideFX/Event Channels/Fade Event Channel")]
	public class FadeEventChannelSO : DescriptionBaseSO {
		/// <summary>
		/// Defines the signature required to subscribe to this event channel.
		/// </summary>
		/// <param name="fadeIn">true to fade gameplay in, false to fade to black</param>
		/// <param name="duration">How long the fade should take</param>
		/// <param name="color">The RGBA Colour to fade to</param>
		public delegate void FadeEvent(bool fadeIn, float duration, Color color);
		public event FadeEvent OnEventRaised = delegate { };

		/// <summary>
		/// Helper function for ease of use. Fades in gameplay.
		/// </summary>
		/// <param name="duration">Time in seconds for fade transition</param>
		public void FadeIn(float duration) {
			Fade(true, duration, Color.clear);
		}

		/// <summary>
		/// Helper function for ease of use. Fades screen out to black
		/// </summary>
		/// <param name="duration">Time in seconds for fade transition</param>
		public void FadeOut(float duration) {
			Fade(false, duration, Color.black);
		}

		public void Fade(bool fadeIn, float duration, Color color) {
			OnEventRaised?.Invoke(fadeIn, duration, color);
		}
	}
}
