using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonPatterns
{
	public class CacellationRequest
	{
		/*
		  Problem
			You have cancelable code (that takes a CancellationToken) and you need to cancel it
		*/

		// this can be
		public static void IssueCancelRequest()
		{
			var cts = new CancellationTokenSource();
			CancelableMethod(cts.Token);
			/*
				when you call the cancel method here
				there is almost always a race condition
			*/
			cts.Cancel();
		}

		// how to timeout using cancellation token
		public static async Task IssueTimeout()
		{
			var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			//cts.CancelAfter(TimeSpan.FromSeconds(5)); this is another way to create timeout token
			await Task.Delay(1000, cts.Token);
		}

		private static void CancelableMethod(CancellationToken cancellationToken)
		{
		}
	}
}