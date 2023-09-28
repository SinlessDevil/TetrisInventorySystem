using System;
using System.Threading.Tasks;
using System.Threading;

namespace Extensions
{
    public static class AsyncExtensions
    {
        public static Task WaitUntilAsync(this CancellationToken cancellationToken, Func<bool> condition)
        {
            var tcs = new TaskCompletionSource<bool>();

            async void AwaitCondition()
            {
                while (!condition())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }

                tcs.SetResult(true);
            }

            AwaitCondition();

            return tcs.Task;
        }
    }
}