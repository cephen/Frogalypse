using SideFX.Events;
using SideFX.SceneManagement;
using SideFX.SceneManagement.Events;

using UnityEngine;
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
		[SerializeField] private SceneData _thisScene;
		[SerializeField] private PersistentManagersScene _persistentManagersScene;

		private bool _isColdStart = false;

		private void Awake() {
			var sceneName = _persistentManagersScene.SceneReference.editorAsset.name;
			if (!SceneManager.GetSceneByName(sceneName).isLoaded)
				_isColdStart = true;
		}

		private void Start() {
			if (_isColdStart) {
				_persistentManagersScene
				.SceneReference
				.LoadSceneAsync(LoadSceneMode.Additive, true)
				.Completed += OnManagersLoaded;
			}
		}

		private void OnManagersLoaded(AsyncOperationHandle<SceneInstance> handle) {
			if (_thisScene != null) {
				// reload the current scene to:
				// - Load gameplay managers
				// - Spawn the player at the player start location
				EventBus<LoadRequest>.Raise(new LoadRequest(_thisScene));
			} else {
				EventBus<SceneReady>.Raise(default);
			}
		}
#endif
	}

}
