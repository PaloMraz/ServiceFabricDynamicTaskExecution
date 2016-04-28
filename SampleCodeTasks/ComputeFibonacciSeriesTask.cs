using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCodeTasks
{
  public sealed class ComputeFibonacciSeriesTask
  {

    public ComputeFibonacciSeriesTask()
    { }

    public async Task<string> ExecuteAsync(string parameters)
    {
      int maxNumber;
      if (!int.TryParse(parameters, out maxNumber))
      {
        return $"Invalid parameter value [{parameters}] - must be an integer.";
      }

      if (maxNumber <= 0)
      {
        return "";
      }

      List<int> series = new List<int>();
      await Task.Run(() => FibonacciSeries(0, 1, 1, maxNumber, series));
      return string.Join(" ", series.Select(n => n.ToString()));
    }


    private static void FibonacciSeries(int firstNumber, int secondNumber, int currentNumber, int maxLength, List<int> series)
    {
      if (currentNumber <= maxLength)
      {
        series.Add(firstNumber);
        FibonacciSeries(secondNumber, firstNumber + secondNumber, currentNumber + 1, maxLength, series);
      }
    }

  }
}
