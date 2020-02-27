using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CommonPatterns
{
    public class LinkingBlocks
    {
        public static async Task JoinBlockGreedy()
        {
            var producer1 = new BufferBlock<int>();
            var producer2 = new BufferBlock<int>();

            var joinBlock = new JoinBlock<int, int>();

            var actionBlock =
                new ActionBlock<Tuple<int, int>>(item =>
                    System.Console.WriteLine(item));

            producer1.LinkTo(joinBlock.Target1, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            producer2.LinkTo(joinBlock.Target2, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            joinBlock.LinkTo(actionBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            producer1.Post(1);
            producer2.Post(5);
            producer1.Post(2);
            producer2.Post(2);

            producer1.Complete();

            // In Greedy mode, the join block will still accept the message but it's not going to show any output because,
            // its second target hasn't received any values
            //producer1.Post(2);
            //producer1.Post(2);
            //producer1.Post(2);

            await actionBlock.Completion;
        }

        public static async Task JoinBlockNonGreedy()
        {
            var producer1 = new BufferBlock<int>();
            var producer2 = new BufferBlock<int>();

            var joinBlock = new JoinBlock<int, int>(
                new GroupingDataflowBlockOptions
                {
                    Greedy = false // because this non greedy
                });

            // this line will never be execute
            var actionBlock =
                new ActionBlock<Tuple<int, int>>(item =>
                    System.Console.WriteLine(item));

            var actionBlockB =
                new ActionBlock<int>(item => Console.WriteLine(item));

            producer1.LinkTo(joinBlock.Target1, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });
            producer2.LinkTo(joinBlock.Target2, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            joinBlock.LinkTo(actionBlock, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            //this action block will suck up all the data before join block has a chance
            producer1.LinkTo(actionBlockB, new DataflowLinkOptions
            {
                PropagateCompletion = true
            });

            producer1.Post(1);
            producer2.Post(5);
            producer1.Post(2);
            producer2.Post(2);

            producer1
                .Post(3); // In non greedymode,the joinblock will postpone receiving this message

            //producer1.Complete();
            //joinBlock.Complete();

            await actionBlock.Completion;

            System.Console.WriteLine("done");
        }

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
            var batchBlock = new BatchBlock<int>(10,
                new GroupingDataflowBlockOptions
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

            System.Console.WriteLine(batchBlock.Receive()
                .Sum()); // this line is unreachable
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

        public static async Task Linking()
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