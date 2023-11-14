using Newtonsoft.Json;

using UnityEngine.AddressableAssets;

namespace SideFX.Scenes {
	/// <summary>
	/// ScriptableObject that contains a scene reference and description.
	/// Enables asynchronous loading and unloading of scenes.
	/// </summary>
	public abstract class SceneDataSO : SerializableScriptableObject {
		[JsonIgnore]
		public AssetReference sceneReference;
	}
}
