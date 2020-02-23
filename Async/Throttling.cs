using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommonPatterns
{
  public class Throttling
  {
    public async static Task<string> Throttle()
    {
      using var client = new HttpClient();
      var nextDelay = TimeSpan.FromSeconds(1);
      for(var i = 0; i != 3; i++) {
        try
        {
          return await client.GetStringAsync("");
        }
        catch
        {

        }
        await Task.Delay(nextDelay);
        nextDelay += nextDelay;
      }

      return await client.GetStringAsync("");
    }
  }
}