using Frogalypse.Events;

using SideFX.Events;

using UnityEngine;

namespace Frogalypse {
	public class SpawnSystem : MonoBehaviour {
		[SerializeField] private PlayerController _playerPrefab;
		private EventBinding<SpawnPlayer> _spawnPlayerBinding;

		private void OnEnable() {
			_spawnPlayerBinding = new EventBinding<SpawnPlayer>(OnSpawnPlayer);
			EventBus<SpawnPlayer>.Register(_spawnPlayerBinding);
		}

		private void OnDisable() {
			EventBus<SpawnPlayer>.Deregister(_spawnPlayerBinding);
		}

		void OnSpawnPlayer(SpawnPlayer @event) {
			if (_playerPrefab == null) {
				Debug.LogError("Player Prefab isn't set, can't spawn player", _playerPrefab);
				return;
			}
			Debug.Log($"Spawning Player at {@event.Position}");
			Instantiate(_playerPrefab, position: @event.Position, Quaternion.identity);
		}
	}
}
