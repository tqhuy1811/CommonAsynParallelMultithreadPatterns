using System;
using System.Linq;
using System.Threading.Tasks;
using CommonPatterns;
using random.ValueTask;

namespace random
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			var valueTask = new ValueTaskBestPractices();
			var result = await valueTask.TestValueTask();
			//var result1 = await valueTask.TestValueTask();
			Console.WriteLine(result);
			//Console.WriteLine(result1);
		}
	}
}