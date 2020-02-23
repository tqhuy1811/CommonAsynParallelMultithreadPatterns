using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonPatterns;

namespace random
{

  class Program
  {
    static async Task Main(string[] args)
    {
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
