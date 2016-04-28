using System;
using System.Linq;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Fabric.Description;
using Microsoft.ServiceFabric.Actors.Client;
using TaskExecutorActors.Interfaces;
using Microsoft.ServiceFabric.Actors;

namespace TaskExecutorGateway.Controllers
{
  [RoutePrefix("gateway/api")]
  public class GatewayController : ApiController
  {
    /// <summary>
    /// Called from TaskExecutorClientLib.TaskExecutorClient.ExecuteTaskAsync.
    /// </summary>
    [Route("execute-task"), HttpPost()]
    public async Task<string> ExecuteTaskAsync([FromBody] Dictionary<string, string> parameters)
    {
      try
      {
        // No error checking - just trusting the TaskExecutorClient :-)
        string taskAssemblyPath = parameters["taskAssemblyPath"];
        string taskClassName = parameters["taskClassName"];
        string taskParameters = parameters["taskParameters"];

        // Generate unique service name based on the assembly directory.
        Uri serviceUri = GetServiceUri(Path.GetDirectoryName(taskAssemblyPath));
        using (var client = new FabricClient())
        {
          Service service = await FindExistingServiceAsync(client, serviceUri);
          if (service == null)
          {
            // Service not yet created - first request for a task from within the directory since the 
            // service started. We now create a new service instance with the unique directory-based name.
            var description = new StatefulServiceDescription()
            {
              ApplicationName = GetApplicationUri(),
              ServiceName = serviceUri,
              ServiceTypeName = "TaskExecutorActorServiceType",
              HasPersistedState = true,
              PartitionSchemeDescription = new UniformInt64RangePartitionSchemeDescription()
            };
            await client.ServiceManager.CreateServiceAsync(description);
          }
        }

        // Now delegate the execution to the service by calling the actor.
        var taskExecutorProxy = ActorProxy.Create<ITaskExecutorActor>(ActorId.CreateRandom(), serviceUri);
        return await taskExecutorProxy.ExecuteTaskAsync(taskAssemblyPath, taskClassName, taskParameters);
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }


    /// <summary>
    /// Called from TaskExecutorClientLib.TaskExecutorClient.UnloadTaskAssembliesAsync.
    /// </summary>
    [Route("unload-task-assemblies"), HttpPost()]
    public async Task<string> UnloadTaskAssembliesAsync([FromBody] Dictionary<string, string> parameters)
    {
      try
      {
        // No error checking - just trusting the TaskExecutorClient :-)
        string assembliesFolderPath = parameters["assembliesFolderPath"];

        Uri serviceUri = GetServiceUri(assembliesFolderPath);
        using (var client = new FabricClient())
        {
          Service service = await FindExistingServiceAsync(client, serviceUri);
          if (service != null)
          {
            await client.ServiceManager.DeleteServiceAsync(serviceUri);
          }
        }
        return "OK";
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }


    private static async Task<Service> FindExistingServiceAsync(FabricClient client, Uri serviceUri)
    {
      ServiceList services = await client.QueryManager.GetServiceListAsync(GetApplicationUri());
      return services.FirstOrDefault(s => s.ServiceName.AbsolutePath.Contains(serviceUri.LocalPath));
    }


    private static Uri GetApplicationUri()
    {
      return new Uri(FabricRuntime.GetActivationContext().ApplicationName);
    }


    private static Uri GetServiceUri(string assembliesFolderPath)
    {
      // Brute force - strip invalid URI segment chars.
      string serviceName = assembliesFolderPath
        .Replace(@"\", "")
        .Replace(":", "")
        .Replace(".", "")
        .Replace("_", "")
        .ToLowerInvariant();

      var builder = new UriBuilder(GetApplicationUri());
      builder.Path += ("/" + serviceName);
      return builder.Uri;
    }
  }
}
