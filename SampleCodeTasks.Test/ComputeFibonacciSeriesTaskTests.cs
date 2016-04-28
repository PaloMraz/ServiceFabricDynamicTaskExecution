using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace SampleCodeTasks.Test
{
  [TestClass]
  public class ComputeFibonacciSeriesTaskTests
  {
    [TestMethod]
    public async Task TestNegativeNumbers()
    {
      var task = new ComputeFibonacciSeriesTask();

      string result = await task.ExecuteAsync("-5");
      Assert.AreEqual("", result);

      result = await task.ExecuteAsync("-1");
      Assert.AreEqual("", result);
    }


    [TestMethod]
    public async Task TestZero()
    {
      var task = new ComputeFibonacciSeriesTask();

      string result = await task.ExecuteAsync("0");
      Assert.AreEqual("", result);
    }


    [TestMethod]
    public async Task TestOne()
    {
      var task = new ComputeFibonacciSeriesTask();

      string result = await task.ExecuteAsync("1");
      Assert.AreEqual("0", result);
    }


    [TestMethod]
    public async Task TestTwo()
    {
      var task = new ComputeFibonacciSeriesTask();

      string result = await task.ExecuteAsync("2");
      Assert.AreEqual("0 1", result);


      result = await task.ExecuteAsync("5");
      Assert.AreEqual("0 1 1 2 3", result);
    }


    [TestMethod]
    public async Task TestFive()
    {
      var task = new ComputeFibonacciSeriesTask();

      string result = await task.ExecuteAsync("5");
      Assert.AreEqual("0 1 1 2 3", result);
    }

  }
}
