using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TaskExecutorClientLib
{
  /// <summary>
  /// Client for calling the TaskExecutorGateway service endpoint.
  /// </summary>
  public sealed class TaskExecutorClient
  {
    private const string JsonContentType = "application/json";
    private readonly string _gatewayApiEndpoint;

    /// <summary>
    /// For local dev cluster use <paramref name="gatewayHostName"/> = localhost.
    /// </summary>
    public TaskExecutorClient(string gatewayHostName)
    {
      if (gatewayHostName == null)
      {
        throw new ArgumentNullException(nameof(gatewayHostName));
      }
      this._gatewayApiEndpoint = $"http://{gatewayHostName}/gateway/api";
    }


    /// <summary>
    /// Executes the specified task on the cluster.
    /// </summary>
    public async Task<string> ExecuteTaskAsync(string taskAssemblyPath, string taskClassName, string taskParameters)
    {
      using (var client = new HttpClient())
      {
        var parameters = new Dictionary<string, string>()
        {
          ["taskAssemblyPath"] = taskAssemblyPath,
          ["taskClassName"] = taskClassName,
          ["taskParameters"] = taskParameters
        };
        string parametersJson = JsonConvert.SerializeObject(parameters);

        var payload = new StringContent(parametersJson, Encoding.UTF8, JsonContentType);
        HttpResponseMessage response = await client.PostAsync(this._gatewayApiEndpoint + "/execute-task", payload);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
      }
    }


    /// <summary>
    /// Unloads the assemblies previously loaded from the given folder.
    /// </summary>
    public async Task<string> UnloadTaskAssembliesAsync(string assembliesFolderPath)
    {
      using (var client = new HttpClient())
      {
        var parameters = new Dictionary<string, string>()
        {
          ["assembliesFolderPath"] = assembliesFolderPath
        };
        string parametersJson = JsonConvert.SerializeObject(parameters);

        var payload = new StringContent(parametersJson, Encoding.UTF8, JsonContentType);
        HttpResponseMessage response = await client.PostAsync(this._gatewayApiEndpoint + "/unload-task-assemblies", payload);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
      }
    }

  }
}
