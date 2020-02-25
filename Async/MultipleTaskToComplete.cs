using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommonPatterns
{
	/*
	  Problem
	  You have several tasks and need to wait for them all to complete.
	*/

	public class MultipletaskToComplete
	{
		public async static Task<string> DownloadAllAsync(IEnumerable<string> urls)
		{
			var httpClient = new HttpClient();
			// Define what we're going to do for each URL.
			var downloads = urls.Select(url => httpClient.GetStringAsync(url));
			// Note that no tasks have actually started yet
			// because the sequence is not evaluated.
			// Start all URLs downloading simultaneously.
			Task<string>[] downloadTasks = downloads.ToArray();
			// Now the tasks have all started.
			// Asynchronously wait for all downloads to complete.
			string[] htmlPages = await Task.WhenAll(downloadTasks);
			return string.Concat(htmlPages);
		}

		/*
		  Exceptions handling
		*/

		private static Task ThrowNowImplmentedExceptionAsync()
		{
			throw new NotImplementedException();
		}

		private static Task ThrowInvalidOperationExceptionAsync()
		{
			throw new InvalidOperationException();
		}

		private static async Task ObserveExceptionAsync()
		{
			try
			{
				await Task.WhenAll(ThrowNowImplmentedExceptionAsync(), ThrowInvalidOperationExceptionAsync());
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e.Message);
				// e here could either be NotImplementedException or InvalidOperationException.
			}
		}

		private static async Task ObserveExceptionAsyncAll()
		{
			var allTasks = Task.WhenAll(ThrowNowImplmentedExceptionAsync(), ThrowInvalidOperationExceptionAsync());
			var allTasksany = await Task.WhenAny(ThrowNowImplmentedExceptionAsync(), ThrowInvalidOperationExceptionAsync());
			try
			{
				await allTasks;
			}
			catch
			{
				// this will contains all exceptions that happens in Task WhenAll
				AggregateException exceptions = allTasks.Exception;
			}
		}
	}
}