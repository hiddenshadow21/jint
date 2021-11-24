using Jint.Runtime;
using System;
using System.Threading;

namespace Jint.Constraints
{
    public sealed class TimeConstraint2 : IConstraint
    {
        public TimeSpan Timeout => _timeout;

        private TimeSpan _timeout;
        private CancellationTokenSource cts;

        public TimeConstraint2(TimeSpan timeout)
        {
            Change(timeout);
        }

        public void Change(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        public void Check()
        {
            if (cts.IsCancellationRequested)
            {
                ExceptionHelper.ThrowTimeoutException();
            }
        }

        public void Reset()
        {
            cts?.Dispose();

            // This cancellation token source is very likely not disposed property, but it only allocates a timer, so not a big deal.
            // But using the cancellation token source is faster because we do not have to check the current time for each statement,
            // which means less system calls.
            cts = new CancellationTokenSource(_timeout);
        }
    }
}
