using Jint.Runtime;
using System;

namespace Jint.Constraints
{
    public sealed class TimeConstraint : IConstraint
    {
        public long MaxTicks => _maxTicks;

        private long _maxTicks;
        private long _timeoutTicks;

        public TimeConstraint(TimeSpan timeout)
        {
            Change(timeout);
        }

        public void Change(TimeSpan timeout)
        {
            _maxTicks = timeout.Ticks;
        }

        public void Check()
        {
            if (_timeoutTicks > 0 && _timeoutTicks < DateTime.UtcNow.Ticks)
            {
                ExceptionHelper.ThrowTimeoutException();
            }
        }

        public void Reset()
        {
            var timeoutIntervalTicks = _maxTicks;

            _timeoutTicks = timeoutIntervalTicks > 0 ? DateTime.UtcNow.Ticks + timeoutIntervalTicks : 0;
        }
    }
}
