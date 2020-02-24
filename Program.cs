using System;
using System.Threading.Tasks;
using CommonPatterns;

namespace random
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			await LinkingBlocks.BatchBlockNonGreedy();
			//await LinkingBlocks.Linking();
			await LinkingBlocks.BroadCastBlock();
			Console.ReadLine();
			//await ProgressReport.CallMyMethodAsync();
			// System.Console.WriteLine("heere");
			// await Task.Delay(5);
			// System.Console.WriteLine("end");
		}
	}
}