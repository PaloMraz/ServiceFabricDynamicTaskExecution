using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskExecutorActors.Interfaces
{
  /// <summary>
  /// Defines contract for and actor able to dynamically load and execute code.
  /// </summary>
  public interface ITaskExecutorActor : IActor
  {
    /// <summary>
    /// Load the specified assembly, instantiate the specified class and call the ExecuteAsync method
    /// with the specified parameters.
    /// </summary>
    Task<string> ExecuteTaskAsync(string taskAssemblyPath, string taskClassName, string taskParameters);
  }
}
