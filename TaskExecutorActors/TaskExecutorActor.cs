using Microsoft.ServiceFabric.Actors.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using TaskExecutorActors.Interfaces;

namespace TaskExecutorActors
{
  /// <summary>
  /// Implements the <see cref="ITaskExecutorActor"/> contract.
  /// </summary>
  [StatePersistence(StatePersistence.Persisted)]
  internal class TaskExecutorActor : Actor, ITaskExecutorActor
  {
    public TaskExecutorActor()
    { }


    /// <summary>
    /// Loads the specified assembly, instantiates the specified class and calls the ExecuteAsync method
    /// with the specified parameters.
    /// </summary>
    public async Task<string> ExecuteTaskAsync(string taskAssemblyPath, string taskClassName, string taskParameters)
    {
      // Sample code: no real error checking!
      try
      {
        ObjectHandle taskObjectHandle = Activator.CreateInstanceFrom(taskAssemblyPath, taskClassName);
        object taskInstance = taskObjectHandle.Unwrap();
        MethodInfo executeMethod = taskInstance.GetType().GetMethod("ExecuteAsync");
        Task<string> executeTask = (Task<string>)executeMethod.Invoke(taskInstance, new object[] { taskParameters });
        await executeTask;
        return executeTask.Result;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }
  }
}
