using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace TaskExecutorActors
{
  internal static class Program
  {
    /// <summary>
    /// This is the entry point of the service host process.
    /// </summary>
    private static void Main()
    {
      // This line registers an Actor Service to host your actor class with the Service Fabric runtime.
      // The contents of your ServiceManifest.xml and ApplicationManifest.xml files
      // are automatically populated when you build this project.
      // For more information, see http://aka.ms/servicefabricactorsplatform

      ActorRuntime.RegisterActorAsync<TaskExecutorActor>(
         (context, actorType) => new ActorService(context, actorType, () => new TaskExecutorActor())).Wait();

      Thread.Sleep(Timeout.Infinite);

    }
  }
}
