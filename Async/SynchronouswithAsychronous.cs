using System.Threading.Tasks;
/*
Problem
You need to implement a synchronous method with an asynchronous signature. This
situation can arise if you are inheriting from an asynchronous interface or base class
but wish to implement it synchronously. This technique is particularly useful when unit
testing asynchronous code, when you need a simple stub or mock for an asynchronous
interface
*/

namespace CommonPatterns
{
  interface IAsyncInterface
  {
    Task<int> DoSomething();
  }
  public class SynchronousWithAsynchronous : IAsyncInterface
  {
    public Task<int> DoSomething()
    {
      return Task.FromResult(13);
    }

    public Task<int> DoSomethingAsyncWithTaskCompletionSource()
    {
      var tcs = new TaskCompletionSource<int>();
      tcs.TrySetResult(3);
      return tcs.Task;
    }



  }
}