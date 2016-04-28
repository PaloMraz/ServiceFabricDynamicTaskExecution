using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SampleWebTasks.Test
{
  [TestClass]
  public class DownloadWebPageTaskTests
  {
    [TestMethod]
    public async Task DownloadStackOverflowQuestionPageTest()
    {
      var task = new DownloadWebPageTask();
      string resuls = await task.ExecuteAsync("http://stackoverflow.com/questions/36845451/service-fabric-actors-in-a-separate-appdomain");
      Assert.IsTrue(resuls.Contains("Service Fabric Actors in a separate AppDomain"));
    }
  }
}
