using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebTasks
{
  public sealed class DownloadWebPageTask
  {
    public DownloadWebPageTask()
    { }

    public async Task<string> ExecuteAsync(string parameters)
    {
      using (var client = new HttpClient())
      {
        HttpResponseMessage response = await client.GetAsync(parameters);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
      }
    }

  }
}
