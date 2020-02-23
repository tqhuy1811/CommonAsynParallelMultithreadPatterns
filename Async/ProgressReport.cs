/*
Problem
You need to respond to progress while an asynchronous operation is executing.
*/

using System;
using System.Threading.Tasks;

namespace CommonPatterns
{
  public class ProgressReport
  {

    // By convention progress maybe null if called don't want receive report
    public async static Task MyMethodAsync(IProgress<int> progress = null)
    {
      int percentComplete = default;
      while (percentComplete != 100)
      {
        // Do something here
        if (progress != null)
        {
          /*
            This method is executing asynchronously => may continue executing before reports appear on caller
            Should use immutable type or at least a value type. 
            If T is a mutuable reference type 
            => need to create a separate copy yourself each time to avoid race condition

          */
          percentComplete += 10;
          progress.Report(percentComplete); 
        }
        
       
      }
      await Task.Yield();
    }
    public static async Task CallMyMethodAsync()
    {
      var progress = new Progress<int>();
      progress.ProgressChanged += (sender, args) =>
      {
        //Progress T will capture the current context when it is constructed and it will invoke its calback within that context
        // => If construct on UI thread then you can update the UI from its callback
        // because this is a Console app therefore the default synchronization context uses the thread pool to invoke delegates,
        // => maynot get deterministically ordered input
        System.Console.WriteLine(args);
        // do something with the report
      };
      await MyMethodAsync(progress);
    }
  }
}