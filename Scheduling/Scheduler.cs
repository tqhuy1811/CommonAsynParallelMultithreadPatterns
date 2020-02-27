using System;
using System.Threading;
using System.Threading.Tasks;

namespace random.Scheduling
{
    /*
     * Problem
     * You have a piece of code that you explicitly want to execute on a thread-pool thread
     */
    public class Scheduler
    {
        public void RunInSpecificThread()
        {
            // ideal for UI app 
            var task =
                Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(2)); });
        }

        public void UseClassicScheduler()
        {
            // create a default task scheduler
            var taskScheduler = TaskScheduler.Default;

            /*
             * Get task scheduler from current sync context
             * use this to schedule work back to the current sync context
             */
            var taskSchedulerWithSyncContext =
                TaskScheduler.FromCurrentSynchronizationContext();
        }

        public void UseNewTaskScheduler()
        {
            var schedulerPair = new ConcurrentExclusiveSchedulerPair();

            /* allow multiple tasks to execute at the same time
             as long as no task is running on exclusive task scheduler
            */
            var concurrent = schedulerPair.ConcurrentScheduler;

            /*
             * allow execute code one task at a time only when there is no task
             * already executing on ConcurrentScheduler
             */
            var exclusive = schedulerPair.ExclusiveScheduler;
        }
    }
}