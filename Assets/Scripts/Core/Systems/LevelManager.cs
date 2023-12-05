using System;

using Frogalypse.Events;
using Frogalypse.Tags;

using SideFX.Anchors;
using SideFX.Events;
using SideFX.SceneManagement.Events;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Frogalypse.Levels {
	internal class LevelManager : MonoBehaviour {
		[SerializeField] private TransformAnchor _playerAnchor;

		private LevelTimer _timer;
		private EventBinding<SceneReady> _sceneReadyBinding;
		private EventBinding<GoalReached> _goalReachedBinding;

		private void Awake() {
			_timer = new LevelTimer(startNow: false);
		}

		private void OnEnable() {
			_sceneReadyBinding = new EventBinding<SceneReady>(OnSceneReady);
			_goalReachedBinding = new EventBinding<GoalReached>(OnGoalReached);
			EventBus<SceneReady>.Register(_sceneReadyBinding);
			EventBus<GoalReached>.Register(_goalReachedBinding);
		}

		private void OnDisable() {
			EventBus<SceneReady>.Deregister(_sceneReadyBinding);
			EventBus<GoalReached>.Deregister(_goalReachedBinding);
		}

		private void OnSceneReady(SceneReady @event) {
			if (CheckGoalZoneExists() && TrySpawnPlayer()) {
				Debug.Log("Starting Timer");
				_timer.Start();
			}
		}

		private void OnGoalReached(GoalReached @event) {
			TimeSpan timeTaken = _timer.Finish();
			EventBus<LevelCompleted>.Raise(new LevelCompleted {
				TimeTaken = timeTaken,
			});
		}

		private bool CheckGoalZoneExists() {
			if (FindFirstObjectByType<GoalZone>() is GoalZone goal && goal != null) {
				Debug.Log($"Found Goal Zone: {goal.transform.position}");
				return true;
			}
			Debug.LogError($"No goal zone in scene:  {SceneManager.GetActiveScene().name}");
			return false;
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

	internal readonly struct LevelCompleted : IEvent {
		public readonly TimeSpan TimeTaken { get; init; }
	}
}
