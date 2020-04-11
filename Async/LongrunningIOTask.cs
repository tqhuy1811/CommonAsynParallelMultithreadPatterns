using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommonPatterns
{
	public class LongrunningIOTask
	{
		/// <summary>
		/// Rough implementation of Longrunning IO task
		/// </summary>
		/// <returns></returns>
		public Task<int> LongrunningTask()
		{
			var tcs = new TaskCompletionSource<int>();
			// doesn't has to be a thread
			new Thread(() =>
					{
						try
						{
							// represent long running io task
							Thread.Sleep(5000);
							tcs.SetResult(42);
						}
						catch (Exception e)
						{
							tcs.SetException(e);
						}
						
					})
				{
					IsBackground = true
				}
				.Start();
			return tcs.Task; // Our "slave" task.
		}
	}
}