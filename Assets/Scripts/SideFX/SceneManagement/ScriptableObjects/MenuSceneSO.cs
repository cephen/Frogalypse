using UnityEngine;
using UnityEngine.SceneManagement;

namespace SideFX.Scenes {
	/// <summary>
	/// This class contains Settings specific to Menus only
	/// </summary>
	[CreateAssetMenu(fileName = "New Menu Scene", menuName = "SideFX/Scene Data/Menu")]
	public class MenuSceneSO : SceneDataSO {
		public LoadSceneMode sceneMode = LoadSceneMode.Single;
	}
}
