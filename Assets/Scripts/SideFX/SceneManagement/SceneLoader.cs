using System.Collections;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

using SideFX.Events;

namespace SideFX.Scenes {
	public class SceneLoader : MonoBehaviour {
		[SerializeField] private PersistentManagersSO _gameplayManagers;

		[Header("Listening to")]
		[SerializeField] private LoadEventChannelSO _coldStartupLocation;
		[SerializeField] private LoadEventChannelSO _loadMenu;
		[SerializeField] private LoadEventChannelSO _loadLocation;

		[Header("Broadcasting on")]
		[SerializeField] private BoolEventChannelSO _toggleLoadingScreen;
		[SerializeField] private VoidEventChannelSO _onSceneReady; //picked up by the SpawnSystem
		[SerializeField] private FadeEventChannelSO _fadeRequestChannel;

		private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
		private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

		private SceneDataSO _sceneToLoad;
		private SceneDataSO _currentlyLoadedScene;
		private bool _showLoadingScreen;

		private SceneInstance _gameplayManagerSceneInstance = new();
		private float _fadeDuration = 0.5f;
		private bool _isLoading = false; // Used to prevent load requests while loads are in progress

		private void OnEnable() {
			_loadMenu.OnLoadingRequested += LoadMenu;
			_loadLocation.OnLoadingRequested += LoadLocation;
#if UNITY_EDITOR
			_coldStartupLocation.OnLoadingRequested += EditorColdStartup;
#endif

		}
		private void OnDisable() {
			_loadMenu.OnLoadingRequested -= LoadMenu;
			_loadLocation.OnLoadingRequested -= LoadLocation;
#if UNITY_EDITOR
			_coldStartupLocation.OnLoadingRequested -= EditorColdStartup;
#endif
		}

#if UNITY_EDITOR
		/// <summary>
		/// Only used when game starts via editor play button
		/// </summary>
		private void EditorColdStartup(SceneDataSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeScreen) {
			Debug.Log("Starting from editor scene, loading gameplay managers");
			_currentlyLoadedScene = currentlyOpenedLocation;

			if (_currentlyLoadedScene is GameplaySceneSO) {
				//Gameplay managers is loaded synchronously
				_gameplayManagerLoadingOpHandle = _gameplayManagers.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
				_gameplayManagerLoadingOpHandle.WaitForCompletion();
				_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

				StartGameplay();
			}
		}
#endif

		private void LoadMenu(SceneDataSO menuToLoad, bool showLoadingScreen, bool fadeScreen) {
			if (_isLoading)
				return;

			if (menuToLoad is not MenuSceneSO) {
				Debug.LogError($"Tried to load non-menu scene {menuToLoad} via MenuLoadEvent");
				return;
			}


			var menuData = menuToLoad as MenuSceneSO;

			switch (menuData.sceneMode) {
				case LoadSceneMode.Single:
					Debug.Log($"Loading menu scene: {menuData}");

					_sceneToLoad = menuData;
					_showLoadingScreen = showLoadingScreen;
					_isLoading = true;

					if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded) {
						Debug.Log($"Unloading scene: {_gameplayManagerSceneInstance.Scene}");
						Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
					}

					StartCoroutine(UnloadPreviousScene());
					break;
				case LoadSceneMode.Additive:
					Debug.Log($"Additively loading menu scene: {menuData}");
					menuData.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, false);
					break;
			}

		}

		private void LoadLocation(SceneDataSO location, bool showLoadingScreen, bool fadeScreen) {
			if (_isLoading)
				return;

			Debug.Log($"Loading location scene: {location.sceneReference}");

			_sceneToLoad = location;
			_showLoadingScreen = showLoadingScreen;
			_isLoading = true;

			// Load gameplay managers if they're not already loaded
			if (_gameplayManagerSceneInstance.Scene == null || !_gameplayManagerSceneInstance.Scene.isLoaded) {
				Debug.Log($"Loading gameplay managers: {_gameplayManagers.sceneReference}");
				_gameplayManagerLoadingOpHandle = _gameplayManagers.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
				_gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
			} else {
				StartCoroutine(UnloadPreviousScene());
			}
		}

		private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> handle) {
			Debug.Log("Gameplay managers loaded");
			_gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

			StartCoroutine(UnloadPreviousScene());
		}

		private IEnumerator UnloadPreviousScene() {
			_fadeRequestChannel.FadeOut(_fadeDuration);

			yield return new WaitForSeconds(_fadeDuration);

			if (_currentlyLoadedScene != null) { // Would be null if game was started via Initialization
				if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid()) {
					//Unload the scene through its AssetReference, i.e. through the Addressable system
					Debug.Log($"Unloading scene: {_currentlyLoadedScene.sceneReference}");
					_currentlyLoadedScene.sceneReference.UnLoadScene();
				}
#if UNITY_EDITOR
			else {
					//Only used when, after a "cold start", the player moves to a new scene
					//Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
					//the scene needs to be unloaded using regular SceneManager instead of as an Addressable
					SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
				}
#endif
			}

			LoadNewScene();
		}

		/// <summary>
		/// Kicks off the asynchronous loading of a scene
		/// </summary>
		private void LoadNewScene() {
			if (_showLoadingScreen)
				_toggleLoadingScreen.RaiseEvent(true);

			_loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
			_loadingOperationHandle.Completed += OnNewSceneLoaded;
		}

		private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> handle) {
			// Save currently loaded scene
			_currentlyLoadedScene = _sceneToLoad;
			Scene s = handle.Result.Scene;
			SceneManager.SetActiveScene(s);

			_isLoading = false;

			if (_showLoadingScreen)
				_toggleLoadingScreen.RaiseEvent(false);

			_fadeRequestChannel.FadeIn(_fadeDuration);

			StartGameplay();
		}

		private void StartGameplay() {
			_onSceneReady.RaiseEvent();
		}

		private void ExitGame() {
			Debug.Log("Exit!");
			Application.Quit();
		}
	}
}
