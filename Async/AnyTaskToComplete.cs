using System.Net.Http;
using System.Threading.Tasks;

namespace CommonPatterns
{


  /*
    Problem
    You have several tasks and need to respond to just one of them completing. The most
    common situation for this is when you have multiple independent attempts at an op‐
    eration, with a first-one-takes-all kind of structure. For example, you could request stock
    quotes from multiple web services simultaneously, but you only care about the first one
    that responds.
  */
  public class AnyTaskToComplete
  {
    public static async Task<int> FirstResponsdingUrlAsync(string urlA, string urlB)
    {
      var httpClient = new HttpClient();
      var downloadTaskA = httpClient.GetByteArrayAsync(urlA);
      var downloadTaskB = httpClient.GetByteArrayAsync(urlB);

      // this will only return the first Task that is completed, that return Task will include exceptions, data 
      var firstCompletedTask = await Task.WhenAny(downloadTaskA, downloadTaskB); 

      var data = await firstCompletedTask;
      return data.Length;
    }


  }

}