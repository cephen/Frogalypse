using System;

using Frogalypse.Events;
using Frogalypse.Levels;
using Frogalypse.Tags;

using SideFX.Events;
using SideFX.SceneManagement;
using SideFX.SceneManagement.Events;

using UnityEngine;

namespace Frogalypse {
	public class GameManager : MonoBehaviour {
		[SerializeField] private MainMenuScene _mainMenuScene;

		private EventBinding<SceneReady> _sceneReadyBinding;
		private EventBinding<GoalReached> _goalReachedBinding;
		private GameplayScene _currentLevel;
		private LevelTimer _levelTimer;

		private void Awake() => _levelTimer = new LevelTimer();

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

		/// <summary>
		/// Handles SceneReady events.
		/// When the loaded scene is a gameplay scene:
		///	- Check if a goal zone exists
		///	- try to spawn the player
		/// - if prior actions succeeded then start the level timer
		/// </summary>
		private void OnSceneReady(SceneReady ready) {
			if (ready.Scene is not GameplayScene scene)
				return;

			_currentLevel = scene;

			if (CheckGoalZoneExists() && TrySpawnPlayer()) {
				_levelTimer.Start();
				Debug.Log($"Starting scene: {_currentLevel.name}");
			}
		}

		private void OnGoalReached(GoalReached goalReached) {
			TimeSpan timeTaken = _levelTimer.Finish();

			/* 
			 * TODO:
			 * - Pause player movement
			 * - Show level end screen
			 * - Save level record
			 */
			EventBus<LevelCompleted>.Raise(new LevelCompleted {
				LevelScene = _currentLevel,
				TimeTaken = timeTaken,
			});
		}

		private bool CheckGoalZoneExists() {
			if (FindFirstObjectByType<GoalZone>() is GoalZone goal && goal != null) {
				Debug.Log($"Found Goal Zone: {goal.transform.position}");
				return true;
			}
			Debug.LogError($"No goal zone in scene:  {_currentLevel.name}");
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
}
