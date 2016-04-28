using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskExecutorClientLib;

namespace TaskExecutorTestConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine(
        "You should really put breakpoints and step through this code only after the SF application is up and running. " +
        "\n\nPress ENTER when the SF app is running...");
      Console.ReadLine();

      string solutionFolderPath = 
        Path.GetFullPath(
          Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
            @"..\..\.."));

      string codeTasksFolderPath = Path.Combine(solutionFolderPath, @"SampleCodeTasks\bin\Debug");
      string codeTasksAssemblyFileName = Path.Combine(codeTasksFolderPath, "SampleCodeTasks.dll");
      string fibonacciCodeTaskClassName = "SampleCodeTasks.ComputeFibonacciSeriesTask";

      string webTasksFolderPath = Path.Combine(solutionFolderPath, @"SampleWebTasks\bin\Debug");
      string webTasksAssemblyFileName = Path.Combine(webTasksFolderPath, "SampleWebTasks.dll");
      string downloadWebPageTaskClassName = "SampleWebTasks.DownloadWebPageTask";

      var client = new TaskExecutorClient("localhost");

      Console.WriteLine($"About to execute {fibonacciCodeTaskClassName}, press ENTER to continue...");
      Console.ReadLine();
      string result = client.ExecuteTaskAsync(codeTasksAssemblyFileName, fibonacciCodeTaskClassName, "10").Result;
      Console.WriteLine($"Executed {fibonacciCodeTaskClassName}, result: {result}\n\n");

      Console.WriteLine($"About to execute {downloadWebPageTaskClassName}, press ENTER to continue...");
      Console.ReadLine();
      result = client.ExecuteTaskAsync(webTasksAssemblyFileName, downloadWebPageTaskClassName, "http://stackoverflow.com/").Result;
      Console.WriteLine($"Executed {downloadWebPageTaskClassName}, result: {result}\n\n");

      Console.WriteLine("About to delete the dynamically created services, press ENTER to continue...");
      Console.ReadLine();
      result = client.UnloadTaskAssembliesAsync(codeTasksFolderPath).Result;
      Console.WriteLine($"Unloaded {codeTasksFolderPath}, result: {result}\n\n");
      result = client.UnloadTaskAssembliesAsync(webTasksFolderPath).Result;
      Console.WriteLine($"Unloaded {webTasksFolderPath}, result: {result}\n\n");

      Console.WriteLine("\n\nPress ENTER key to close...");
      Console.ReadLine();
    }

  }
}
