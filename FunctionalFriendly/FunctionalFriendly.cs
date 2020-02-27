using System;
using System.Threading;
using System.Threading.Tasks;

namespace random.FunctionalFriendly
{
    /*
      Problem
        You have a type that allows asynchronous operations but also needs to allow disposal
        of its resources.
     */
    public class FunctionalFriendly : IDisposable
    {
        private readonly CancellationTokenSource _disposeCts = new
            CancellationTokenSource();

        public void Dispose()
        {
            _disposeCts.Cancel();
        }

        public async Task<int> CalculateValueAsync(CancellationToken cts)
        {
            // combine current token with supplied token from the user
            CancellationTokenSource.CreateLinkedTokenSource(cts,
                _disposeCts.Token);
            await Task.Delay(TimeSpan.FromSeconds(2), _disposeCts.Token);
            return 13;
        }
    }
}