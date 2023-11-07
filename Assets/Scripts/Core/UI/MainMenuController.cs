using UnityEngine;
using UnityEngine.UIElements;

namespace Frogalypse {
	[RequireComponent(typeof(UIDocument))]
	internal sealed class MainMenuController : MonoBehaviour {
		private UIDocument _doc;
		private Button _startGameButton;
		private Button _settingsButton;
		private Button _exitGameButton;

		private void Awake() {
			_doc = GetComponent<UIDocument>();
			_startGameButton = _doc.rootVisualElement.Q<Button>("start-game");
			_settingsButton = _doc.rootVisualElement.Q<Button>("settings");
			_exitGameButton = _doc.rootVisualElement.Q<Button>("exit-game");
		}

		private void OnEnable() {
			_exitGameButton.clicked += OnExitGame;
		}

		private void OnDisable() {
			_exitGameButton.clicked -= OnExitGame;
		}

		private void OnExitGame() {
			Debug.Log("Exiting Game");
			Application.Quit();
		}


	}
}
