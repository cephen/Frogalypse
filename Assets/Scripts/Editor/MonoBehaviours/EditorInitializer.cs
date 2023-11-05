using SideFX.Events;
using SideFX.Scenes;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Frogalypse.Editor {
	/// <summary>
	/// Loads persistent managers and gameplay managers in the editor if not already loaded
	/// (e.g. when the play button is pressed while editing a scene)
	/// </summary>
	internal sealed class EditorInitializer : MonoBehaviour {
#if UNITY_EDITOR
		[SerializeField] private SceneDataSO _thisScene;
		[SerializeField] private PersistentManagersSO _persistentManagersScene;
		[SerializeField] private AssetReferenceT<LoadEventChannelSO> _notifyColdStartupChannel;
		[SerializeField] private VoidEventChannelSO _onSceneReadyChannel;

		private bool _isColdStart = false;

		private void Awake() {
			// If the persistent managers aren't loaded, this is an editor session and thus a cold start
			string sceneName = _persistentManagersScene.sceneReference.editorAsset.name;
			if (!SceneManager.GetSceneByName(sceneName).isLoaded) { _isColdStart = true; }
		}

		private void Start() {
			if (_isColdStart) {
				_persistentManagersScene
					.sceneReference
					.LoadSceneAsync(LoadSceneMode.Additive, activateOnLoad: true)
					.Completed += OnManagersLoaded;
			}
		}

		private void OnManagersLoaded(AsyncOperationHandle<SceneInstance> sceneHandle) {
			_notifyColdStartupChannel
			.LoadAssetAsync<LoadEventChannelSO>()
			.Completed += OnNotifyChannelLoaded;
		}

		private void OnNotifyChannelLoaded(AsyncOperationHandle<LoadEventChannelSO> channelHandle) {
			if (_thisScene != null) {
				channelHandle.Result.RaiseEvent(_thisScene);
			} else {
				// Raise a fake SceneReady event, so dependent scripts can start
				_onSceneReadyChannel.RaiseEvent();
			}
		}



#endif
	}

}
