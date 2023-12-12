using SideFX;

using UnityEngine;

namespace Frogalypse.Data {
	internal abstract class ScriptableVariable<T> : DescribedScriptable where T : struct {
		[field: SerializeField] public T Value { get; private set; }
	}
}
