using DG.Tweening;

using Frogalypse.Levels;
using Frogalypse.UI;

using UnityEngine;
using UnityEngine.UIElements;

namespace Frogalypse {
	[RequireComponent(typeof(UIDocument))]
	internal sealed class MainMenuController : MonoBehaviour {
		[SerializeField] private LevelDB _levels;

		private UIDocument _doc;
		private VisualElement _sidebar;
		private Button _startGameButton;
		private Button _settingsButton;
		private Button _exitGameButton;
		private Button _saveSettingsButton;
		private Button _exitSettingsButton;
		private LevelSelector _levelSelector;

		private MenuState _currentState = MenuState.Index;
		private MenuState? _nextState = null;
		private bool _isTransitioning = false;

		[SerializeField] private float _scrollTime = 0.5f;
		private float _scrollHeight = 0f;


		private enum MenuState : byte {
			Index, LevelSelection, Settings
		}

		private void Awake() {
			_doc = GetComponent<UIDocument>();
			_sidebar = _doc.rootVisualElement.Q<VisualElement>("sidebar");

			_startGameButton = _doc.rootVisualElement.Q<Button>("start-game");
			_settingsButton = _doc.rootVisualElement.Q<Button>("settings");
			_exitGameButton = _doc.rootVisualElement.Q<Button>("exit-game");

			_saveSettingsButton = _doc.rootVisualElement.Q<Button>("save-settings");
			_exitSettingsButton = _doc.rootVisualElement.Q<Button>("exit-settings");

			_levelSelector = _doc.rootVisualElement.Q<LevelSelector>("level-selector");
			_levelSelector.Populate(_levels);
		}

		private void Update() {
			if (!_isTransitioning) {
				CheckTransitions();
			} else {
				if (_nextState is MenuState.Index or MenuState.Settings) {
					_sidebar.style.bottom = new StyleLength(new Length(_scrollHeight, LengthUnit.Percent));
				}
			}
		}

		private void OnEnable() {
			_startGameButton.clicked += NavigateToLevelSelect;
			_settingsButton.clicked += NavigateToSettings;
			_exitGameButton.clicked += ExitGame;

			_saveSettingsButton.clicked += NavigateToIndex; // TODO: also save settings
			_exitSettingsButton.clicked += NavigateToIndex;
		}

		private void OnDisable() {
			_startGameButton.clicked -= NavigateToLevelSelect;
			_settingsButton.clicked -= NavigateToSettings;
			_exitGameButton.clicked -= ExitGame;

			_saveSettingsButton.clicked -= NavigateToIndex;
			_exitSettingsButton.clicked -= NavigateToIndex;
		}

		private void NavigateToIndex() => _nextState ??= MenuState.Index;
		private void NavigateToSettings() => _nextState ??= MenuState.Settings;
		private void NavigateToLevelSelect() => _nextState ??= MenuState.LevelSelection;

		private void ExitGame() {
			Debug.Log("Exiting Game");
			Application.Quit();
		}

		private void CheckTransitions() {
			switch (_currentState, _nextState) {
				case (_, null): // No transition queued
					return;
				case (MenuState.Index, MenuState.Settings): // Index => Settings
					DOTween.To(
						getter: () => _scrollHeight,
						setter: (float x) => _scrollHeight = x,
						endValue: 100f,
						duration: _scrollTime)
						.OnComplete(() => {
							_currentState = MenuState.Settings;
							_nextState = null;
							_isTransitioning = false;
						});
					_isTransitioning = true;
					return;
				case (MenuState.Settings, MenuState.Index): // Settings => Index
					DOTween.To(
						getter: () => _scrollHeight,
						setter: (float x) => _scrollHeight = x,
						endValue: 0f,
						duration: _scrollTime)
						.OnComplete(() => {
							_currentState = MenuState.Index;
							_nextState = null;
							_isTransitioning = false;
						});
					_isTransitioning = true;
					return;
			}
		}
	}
}
