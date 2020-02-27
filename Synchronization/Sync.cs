using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace random.Synchronization
{
    /*
     * Problem
     * You have some shared data and the need to safety read write it from multiple thread
     */
    public class Sync
    {
        private readonly object _mutex = new object();

        private int _value;

        public void Increment()
        {
            /*
             * There are many kind of locks such as:
             * Monitor, SpinLock, ReaderWriterLockSlim
             * should never be used in most application
             */
            lock (_mutex)
            {
                _value = _value + 1;
            }
        }
    }

    /*
     * You have some shared data and need to safely read and write it from multiple code blocks which maybe using await
     */
    public class AsyncLock
    {
        // initialCount = initial number of requests that can be granted concurrently
        // can only be used on .NET 4.5 and above
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        private int _value;

        private async Task DelayAndIncrementAsync()
        {
            await _mutex.WaitAsync();
            try
            {
                var oldValue = _value;
                await Task.Delay(TimeSpan.FromSeconds(oldValue));
                _value = oldValue + 1;
            }
            finally
            {
                _mutex.Release();
            }
        }
    }

    /*
     * Problem
     * You have to send a notification from one thread to another
     */
    public class BlockingSignals
    {
        // Can only be in one of 2 states
        private readonly ManualResetEventSlim _initialized =
            new ManualResetEventSlim();

        private int _value;

        // Any thread may set the event to a signaled state
        public void InitializeFromAnotherThread()
        {
            _value = 13;
            _initialized.Set();
        }

        // or reset the event to an unsignaled state
        public void ResetFromAnotherThread()
        {
            _initialized.Reset();
        }


        public int WaitForIntialization()
        {
            _initialized.Wait();
            return _value;
        }
    }

    /*
     * Throttling request
     */
    public class Throttling
    {
        public async Task<string[]> DownloadUrlsAsync(IEnumerable<string> urls)
        {
            var httpClient = new HttpClient();
            var semaphore = new SemaphoreSlim(10);
            var tasks = urls.Select(async url =>
            {
                await semaphore.WaitAsync();
                try
                {
                    return await httpClient.GetStringAsync(url);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();
            return await Task.WhenAll(tasks);
        }
    }
}