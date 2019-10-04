using AsyncCalculator;
using System;

namespace AsyncAvaitTask1
{
    public class Program
    {
        const string PleaseEnterNumber = "Please enter number or ESC for exit.";

        static void Main(string[] args)
        {
            var starter = new AsyncSumStarter();
            Console.WriteLine(PleaseEnterNumber);
            starter.ReadUserInput();
            starter.StartSumCount();
        }
    }
}
