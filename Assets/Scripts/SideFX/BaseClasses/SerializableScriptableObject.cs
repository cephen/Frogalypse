#if UNITY_EDITOR
using UnityEditor;
#endif

using Newtonsoft.Json;

using UnityEngine;

namespace SideFX {
	public class SerializableScriptableObject : ScriptableObject {
		[SerializeField, HideInInspector] private string _guid;
		[JsonIgnore]
		public string Guid => _guid;

#if UNITY_EDITOR
		private void OnValidate() {
			string path = AssetDatabase.GetAssetPath(this);
			_guid = AssetDatabase.AssetPathToGUID(path);
		}
#endif
	}
}
