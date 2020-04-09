using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CommonPatterns
{
	public class AsyncStateMachine
	{
		internal static async Task<T> Async<T>(T value)
		{
			var value1 = Start(value);

			var result1 = await Async1(value1);

			var value2 = Continuation1(result1);

			var result2 = await Async2(value2);

			var value3 = Continuation2(result2);

			var result3 = await Async3(value3);

			var result = Continuation3(result3);

			return result;
		}

		internal static T Start<T>(T value)
		{
			return value;
		}


		internal static Task<T> Async1<T>(T value)
		{
			return Task.Run(() => value);
		}


		internal static T Continuation1<T>(T value)
		{
			return value;
		}


		internal static Task<T> Async2<T>(T value)
		{
			return Task.FromResult(value);
		}


		internal static T Continuation2<T>(T value)
		{
			return value;
		}


		internal static Task<T> Async3<T>(T value)
		{
			return Task.Run(() => value);
		}


		internal static T Continuation3<T>(T value)
		{
			return value;
		}

		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct AsyncStateMachineGen<TResult> : IAsyncStateMachine
		{
			public int State;
			public AsyncTaskMethodBuilder<TResult> Builder;
			public TResult Value;
			private TaskAwaiter<TResult> awaiter;
			
			void IAsyncStateMachine.MoveNext()
			{
				TResult result;
				try
				{ 
					switch (this.State)
					{
						case -1: // Start code from the beginning to the 1st await.
							// Workflow begins.
							TResult value1 = Start(this.Value);
							this.awaiter = Async1(value1).GetAwaiter();
							if (this.awaiter.IsCompleted)
							{
								// If the task returned by Async1 is already completed, immediately execute the continuation.
								goto case 0;
							}

							else
							{
								this.State = 0;
								// If the task returned by Async1 is not completed, specify the continuation as its callback.
								this.Builder.AwaitUnsafeOnCompleted(ref this.awaiter, ref this);
								// Later when the task returned by Async1 is completed, it calls back MoveNext, where State is 0.
								return;
							}
						case 0: // Continuation code from after the 1st await to the 2nd await.
							// The task returned by Async1 is completed. The result is available immediately through GetResult.
							TResult result1 = this.awaiter.GetResult();
							TResult value2 = Continuation1(result1);
							this.awaiter = Async2(value2).GetAwaiter();
							if (this.awaiter.IsCompleted)
							{
								// If the task returned by Async2 is already completed, immediately execute the continuation.
								goto case 1;
							}
							else
							{
								this.State = 1;
								// If the task returned by Async2 is not completed, specify the continuation as its callback.
								this.Builder.AwaitUnsafeOnCompleted(ref this.awaiter, ref this);
								// Later when the task returned by Async2 is completed, it calls back MoveNext, where State is 1.
								return;
							}
						case 1: // Continuation code from after the 2nd await to the 3rd await.
							// The task returned by Async2 is completed. The result is available immediately through GetResult.
							TResult result2 = this.awaiter.GetResult();
							TResult value3 = Continuation2(result2);
							this.awaiter = Async3(value3).GetAwaiter();
							if (this.awaiter.IsCompleted)
							{
								// If the task returned by Async3 is already completed, immediately execute the continuation.
								goto case 2;
							}
							else
							{
								this.State = 2;
								// If the task returned by Async3 is not completed, specify the continuation as its callback.
								this.Builder.AwaitUnsafeOnCompleted(ref this.awaiter, ref this);
								// Later when the task returned by Async3 is completed, it calls back MoveNext, where State is 1.
								return;
							}
						case 2: // Continuation code from after the 3rd await to the end.
							// The task returned by Async3 is completed. The result is available immediately through GetResult.
							TResult result3 = this.awaiter.GetResult();
							result = Continuation3(result3);
							this.State = -2; // -2 means end.
							this.Builder.SetResult(result);
							// Workflow ends.
							return;
					}
				}
				catch (Exception exception)
				{
					this.State = -2; // -2 means end.
					this.Builder.SetException(exception);
				}
			}
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine asyncStateMachine) =>
				this.Builder.SetStateMachine(asyncStateMachine);
		}
	}
}