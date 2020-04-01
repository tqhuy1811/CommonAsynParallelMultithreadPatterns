using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace random.ValueTask
{
	/// <summary>
	/// Great article https://devblogs.microsoft.com/dotnet/understanding-the-whys-whats-and-whens-of-valuetask/
	/// Read the article to understand the usage of ManualResetValueTaskSourceCore
	/// https://github.com/dotnet/runtime/issues/27558
	/// </summary>
	public class ValueTaskSource : IValueTaskSource<int>
	{
		public ManualResetValueTaskSourceCore<int> Val { get; set; }

		public ValueTaskSource(ManualResetValueTaskSourceCore<int> val)
		{
			Val = val;
		}
		
		public int GetResult(short token)
		{
			return Val.GetResult(token);
		}

		public ValueTaskSourceStatus GetStatus(short token)
		{
			return Val.GetStatus(token);
		}

		public void OnCompleted(Action<object?> continuation,
			object? state,
			short token,
			ValueTaskSourceOnCompletedFlags flags)
		{
			Val.OnCompleted(continuation, state, token, flags);
		}
	}
	public class ValueTaskBestPractices
	{
		// Main reason why value task can't be await multiple time
		public async ValueTask<int> TestValueTask()
		{
			var manualResetValue = new ManualResetValueTaskSourceCore<int>();
			manualResetValue.SetResult(10);
			var valueTaskSource = new ValueTaskSource(manualResetValue);
			var valueTask = await new ValueTask<int>(valueTaskSource, 0);
			
			// valuetasksource doesn't has to be reallocated 
			manualResetValue.Reset();
			valueTaskSource.Val = manualResetValue;
			manualResetValue.SetResult(20);
			// new task with new value
			// this is why ValueTask can't be await twice, it could work but it also doesn't work
			var secondValueTask = new ValueTask<int>(valueTaskSource,1);
			// for complete and advance usage see this http://tooslowexception.com/implementing-custom-ivaluetasksource-async-without-allocations/
			return await secondValueTask;
		}
		
		public async Task TestValueTask2()
		{
			var result = await TestValueTask3(); // Good
			var result2 = await TestValueTask3().AsTask(); // Good, Covert to task
			
			
			// Really bad, because value task could be reuse by then and it could represent a different state
			// Also don't use Task.WhenAll or Task.WhenAny, if there's a need for it then should convert to Task
			var result3 = await TestValueTask3();
			var result4 = await TestValueTask3();
			
			// Don't block more info on https://blog.stephencleary.com/2020/03/valuetask.html
			var result5 = TestValueTask3().Result;
			
			
			
		}
		
		public ValueTask<int> TestValueTask3()
		{
			return new ValueTask<int>(20);
		}
	}
}