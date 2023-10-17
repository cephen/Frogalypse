using UnityEngine;
using System;

namespace SideFX {
	/// <summary>
	/// Anchors are shared references to important game objects that you can place in the project!
	/// They are used as a modular way to communicate between systems,
	/// as an alternative to GameObject.Find() or similar lookups
	/// </summary>
	/// <typeparam name="T">The type of data the anchor stores</typeparam>
	public class AnchorBase<T> : DescriptionBaseSO where T : UnityEngine.Object {
		/// <summary>
		/// Event that fires when a new reference is provided to the anchor.
		/// </summary>
		public event Action OnAnchorUpdated = delegate { };

		/// <summary>
		/// Whether the anchor contains a value.
		/// </summary>
		public bool IsSet { get; private set; } = false;

		[SerializeField] private T _value;
		public T Value => _value;


		/// <summary>
		/// Set the contents of the anchor. Will log an error if null is provided.
		/// </summary>
		/// <param name="value">Data to store in the anchor.</param>
		public void Provide(T value) {
			if (value is null) {
				Debug.LogError($"A null value was provided to the {name} Runtime Anchor! If you want to clear the anchor use Unset() instead.");
				return;
			}
			_value = value;
			IsSet = true;

			OnAnchorUpdated?.Invoke();
		}

		/// <summary>
		/// Clear the anchor.
		/// </summary>
		public void Unset() {
			_value = null;
			IsSet = false;
		}

		private void OnDisable() {
			Unset();
		}
	}
}
