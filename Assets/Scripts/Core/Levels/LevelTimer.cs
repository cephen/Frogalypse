using System;

namespace Frogalypse.Levels {
	internal struct LevelTimer {
		private DateTime _startTime;
		private DateTime _endTime;
		private State _state;

		internal readonly TimeSpan Elapsed => _state switch {
			State.Running => DateTime.UtcNow - _startTime,
			State.Stopped => _endTime - _startTime,
			_ => throw new ArgumentOutOfRangeException(nameof(_state)),
		};

		internal LevelTimer(bool startNow) {
			_startTime = DateTime.UtcNow;
			_endTime = DateTime.UtcNow;
			_state = State.Stopped;

			if (startNow)
				Start();
		}

		internal enum State {
			Running,
			Stopped,
		}

		internal void Start() {
			_startTime = DateTime.UtcNow;
			_state = State.Running;
		}

		internal TimeSpan Finish() {
			switch (_state) {
				case State.Running:
					_endTime = DateTime.UtcNow;
					_state = State.Stopped;
					return Elapsed;
				default:
					return Elapsed;
			}
		}
	}
}
