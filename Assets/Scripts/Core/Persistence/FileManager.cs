using System;
using System.IO;

using UnityEngine;

namespace Frogalypse.Persistence {
	public static class FileManager {
		public static bool WriteToFile(string fileName, string contents) {
			var fullPath = Path.Combine(Application.persistentDataPath, fileName);

			try {
				File.WriteAllText(fullPath, contents);
				return true;
			} catch (Exception e) {
				Debug.LogError($"Failed to write to {fullPath} with exception {e}");
				return false;
			}
		}

		public static bool LoadFromFile(string fileName, out string result) {
			var fullPath = Path.Combine(Application.persistentDataPath, fileName);

			if (!File.Exists(fullPath))
				File.WriteAllText(fullPath, string.Empty);

			try {
				result = File.ReadAllText(fullPath);
				return true;
			} catch (Exception e) {
				Debug.LogError($"Failed to read from {fullPath} with exception {e}");
				result = string.Empty;
				return false;
			}
		}

		public static bool MoveFile(string from, string to) {
			var fromPath = Path.Combine(Application.persistentDataPath, from);
			var toPath = Path.Combine(Application.persistentDataPath, to);
			try {
				File.Move(fromPath, toPath);
				return true;
			} catch (Exception e) {
				Debug.LogError($"Failed to move {fromPath} to {toPath} with exception {e}");
				return false;
			}
		}
	}
}
