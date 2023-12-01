using Frogalypse.Events;

using SideFX.Anchors;
using SideFX.Events;
using SideFX.SceneManagement.Events;

using UnityEngine;

namespace Frogalypse.Levels {
	internal class LevelManager : MonoBehaviour {
		[field: SerializeField] internal Transform PlayerStartPosition { get; private set; } = default;
		[field: SerializeField] internal Collider2D GoalZone { get; private set; }

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

		private void OnDrawGizmos() {
			Gizmos.color = Color.green;
			Vector2 spawnPos = (Vector2) PlayerStartPosition.position;
			Gizmos.DrawLine(spawnPos, spawnPos + Vector2.up);
		}

		private void OnSceneReady(SceneReady @event) {
			Debug.Log("Spawning Player");
			EventBus<SpawnPlayer>.Raise(default);

			Debug.Log("Starting Timer");
			_timer.Start();
		}
	}
}
