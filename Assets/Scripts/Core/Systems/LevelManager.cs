using Frogalypse.Events;
using Frogalypse.Tags;

using SideFX.Anchors;
using SideFX.Events;
using SideFX.SceneManagement.Events;

using UnityEngine;

namespace Frogalypse.Levels {
	internal class LevelManager : MonoBehaviour {
		[SerializeField] private TransformAnchor _playerAnchor;

		private LevelTimer _timer;
		private EventBinding<SceneReady> _sceneReadyBinding;

		private void Awake() {
			_timer = new LevelTimer(startNow: false);
		}

		private void OnEnable() {
			_sceneReadyBinding = new EventBinding<SceneReady>(OnSceneReady);
			EventBus<SceneReady>.Register(_sceneReadyBinding);
		}

		private void OnDisable() {
			EventBus<SceneReady>.Deregister(_sceneReadyBinding);
		}

		private void OnSceneReady(SceneReady @event) {
			TrySpawnPlayer();

			Debug.Log("Starting Timer");
			_timer.Start();
		}

		private bool TrySpawnPlayer() {
			if (FindFirstObjectByType<PlayerStart>() is PlayerStart start && start != null) {
				Debug.Log("Spawning Player");
				EventBus<SpawnPlayer>.Raise(new(start.transform.position));
				return true;
			}

			Debug.LogError("No player start object found", this);
			return false;
		}
	}
}
