using System.Threading.Tasks;

namespace CommonPatterns
{
  /*
  Problem
    You have a number of methods to call in parallel, and these methods are (mostly) in‐
    dependent of each other.
  */

  public class ParallelInvocation
  {
    static void ProcessArray(double[] array)
    {
      Parallel.Invoke(
      () => ProcessPartialArray(array, 0, array.Length / 2),
      () => ProcessPartialArray(array, array.Length / 2, array.Length)
      );
    }
    static void ProcessPartialArray(double[] array, int begin, int end)
    {
      // CPU-intensive processing...
    }
  }
}