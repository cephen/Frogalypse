using System;

using UnityEngine;

using SideFX.Scenes;

namespace SideFX.Events {
	[CreateAssetMenu(fileName = "SceneLoadEvent", menuName = "SideFX/Event Channels/Scene Load Event Channel")]
	public class LoadEventChannelSO : DescriptionBaseSO {
		public event Action<SceneDataSO, bool, bool> OnLoadingRequested = delegate { };

		public void RaiseEvent(SceneDataSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false) {
			if (OnLoadingRequested == null) {
				Debug.LogWarning("A Scene load was requested, but no SceneLoader was registered. Cancelling.");
				return;
			}

			OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen, fadeScreen);
		}
	}
}
