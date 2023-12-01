using System;

namespace Frogalypse.Levels {
	internal struct LevelTimer {
		private DateTime _startTime;
		private DateTime _endTime;
		private State _state;

		internal readonly TimeSpan Elapsed => _endTime - _startTime;

		internal LevelTimer(bool startNow) {
			_startTime = DateTime.UtcNow;
			_endTime = DateTime.UtcNow;
			_state = State.Inactive;

			if (startNow)
				Start();
		}

		internal enum State {
			Active,
			Inactive,
		}

		internal void Start() {
			_startTime = DateTime.UtcNow;
			_state = State.Active;
		}

		internal TimeSpan Finish() {
			switch (_state) {
				case State.Active:
					_endTime = DateTime.Now;
					_state = State.Inactive;
					return Elapsed;
				default:
					return Elapsed;
			}
		}
	}
}
