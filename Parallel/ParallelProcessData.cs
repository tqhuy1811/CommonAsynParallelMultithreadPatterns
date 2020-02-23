using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommonPatterns
{
  /*
   Problem
    You have a collection of data and you need to perform the same operation on each
    element of the data. This operation is CPU-bound and may take some time.
  */

  public class Matrix
  {
    public bool IsInvertible()
    {
      return true;
    }
  }

  public class ParallelProcessData
  {
    void InvertMatrices(IEnumerable<Matrix> matrices)
    {
      /*
       this is how to break out of the Parallel loop
        Parallel task may run on a different thread => any shared state must be protect
        you can also use PLINQ but the main difference is PLINQ will use all of your cpu cores 
        while Parallel will dynamicall react
      */
      Parallel.ForEach(matrices, (matrix, state) =>
      {
        if (!matrix.IsInvertible())
        {
          state.Stop();
        }
        else
        {

        }
      });
    }

    void InvertMatricesCancel(IEnumerable<Matrix> matrices)
    {

      // this is how to cancel  Parallel loop
      var cancelToken = new CancellationTokenSource();
      
      Parallel.ForEach(matrices, new ParallelOptions
      {
        CancellationToken = cancelToken.Token
      }, (matrix) => {
        //do something
      });
    }
  }
}