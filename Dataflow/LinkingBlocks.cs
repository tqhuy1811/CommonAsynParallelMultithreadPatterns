using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CommonPatterns
{
	/*
		You need to link dataflow into each other to create a mesh
	*/

	public class LinkingBlocks
	{
		public static void BatchBlock()
		{
			var batchBlock = new BatchBlock<int>(10);

			for (int i = 0; i < 13; i++)
			{
				batchBlock.Post(i);
			}

			batchBlock.Complete();

			System.Console.WriteLine(batchBlock.Receive().Sum());
			System.Console.WriteLine(batchBlock.Receive().Sum());
		}

		public static async Task BatchBlockNonGreedy()
		{
			var inputBuffer = new BufferBlock<int>();

			// because we have batch size of 10 and using Non Greedy mode => we need 10 different sources to for BatchBlock to consume all of the messages
			var batchBlock = new BatchBlock<int>(10, new GroupingDataflowBlockOptions
			{
				Greedy = false
			});

			inputBuffer.LinkTo(batchBlock, new DataflowLinkOptions
			{
				PropagateCompletion = true
			});

			for (int i = 0; i < 9; i++)
			{
				inputBuffer.Post(i);
			}

			inputBuffer.Complete();

			await batchBlock.Completion;

			System.Console.WriteLine(batchBlock.Receive().Sum()); // this line is unreachable
		}

		public static async Task BroadCastBlock()
		{
			var broadcastBlock = new BroadcastBlock<double>(val =>
			{
				return val * 2;
			});

			var actionBlock = new ActionBlock<double>(val =>
			{
				System.Console.WriteLine(val);
			});

			broadcastBlock.LinkTo(actionBlock, new DataflowLinkOptions
			{
				PropagateCompletion = true
			});

			broadcastBlock.Post(10);
			broadcastBlock.Post(20);
			broadcastBlock.Post(30);

			broadcastBlock.Complete();
			await actionBlock.Completion;
			System.Console.WriteLine("Done");
		}

		public async static Task Linking()
		{
			var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
			var subtractBlock = new TransformBlock<int, int>(item =>
			{
				return item - 2;
			});

			multiplyBlock.LinkTo(subtractBlock, new DataflowLinkOptions
			{
				PropagateCompletion = true
			});

			foreach (var item in Enumerable.Repeat(1, 10).ToArray())
			{
				multiplyBlock.Post(item);
			}

			multiplyBlock.Complete();
			/*
			  Microsoft documentation is kinda misleading about waiting for the final block to complete
			  whether a block is completed or not depends on the type of the block
			  TransformBlock needs 3 conditions
			  1/ TransformBlock.Complete() has been called
			  2/ InputCount == 0 the block has applied its transformation to every incoming element
			  3/ Outputcount == 0 all transformed elements have left the output buffer
			*/
			await subtractBlock.Completion;

			System.Console.WriteLine("Done"); // this part is unreachable
		}
	}
}