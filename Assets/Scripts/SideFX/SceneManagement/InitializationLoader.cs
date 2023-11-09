using SideFX.Events;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SideFX.Scenes {
	/// <summary>
	/// Responsible for strting the game by:
	/// - Loading the persistent managers
	/// - Raising the event to load the Main Menu
	/// </summary>
	public class InitializationLoader : MonoBehaviour {
		[SerializeField] private PersistentManagersSO _managersScene = default;
		[SerializeField] private MenuSceneSO _mainMenuScene = default;
		[Header("Broadcasting on")]
		[SerializeField] private AssetReferenceT<LoadEventChannelSO> _menuLoadChannel = default;

		private void Start() {
			Debug.Log($"Loading Persistent Managers from Scene Data: {_managersScene}");
			_managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
		}

		private void LoadEventChannel(AsyncOperationHandle<SceneInstance> handle) {
			// Managers are loaded, load event channels & send LoadSceneEvent
			_menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
		}

		private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> handle) {
			Debug.Log($"Loading Main Menu from Scene Data: {_mainMenuScene}");
			handle.Result.RaiseEvent(_mainMenuScene, true);

			//Initialization is the only scene in BuildSettings, thus it has index 0
			SceneManager.UnloadSceneAsync(0);
		}
	}
}
