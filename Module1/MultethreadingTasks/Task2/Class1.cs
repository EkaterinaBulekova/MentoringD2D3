using System;
using System.Linq;
using System.Threading.Tasks;
using TasksInterface;

namespace Task2
{
    public class ArrayProcessor : IStartable
    {
        private const int MaxValue = 100;
        private readonly int _length;
        private readonly Random _random;

        public ArrayProcessor(int arrayLength)
        {
            _length = arrayLength;
            _random = new Random();
        }

        public void ProcessChain()
        {
            var getData = Task.Factory.StartNew(() => GetData(_length));

            var multipleData = getData.ContinueWith((x) => MultipleData(x.Result));

            var sortData = multipleData.ContinueWith((x) => SortData(x.Result));

            var averageOfData = sortData.ContinueWith((x) => GetAverageOfData(x.Result));

            Task.WaitAll(new Task[] { getData, multipleData, sortData, averageOfData });
        }

        private int[] GetData(int count)
        {
            var values = new int[count];
            values = values.Select(x => _random.Next(MaxValue)).ToArray();
            PrintData("Start array", values);
            return values;
        }

        private int[] MultipleData(int[] values)
        {
            var multiplier = _random.Next(MaxValue);
            PrintData("Multiplier", multiplier);
            values = values.Select(x => x * multiplier).ToArray();
            PrintData("Result array", values);
            return values;
        }

        private int[] SortData(int[] values)
        {
            values = values.OrderBy(x => x).ToArray();
            PrintData("Sorted array", values);
            return values;
        }

        private double GetAverageOfData(int[] values)
        {
            var average = values.Average();
            PrintData("Average value", average);
            return average;
        }

        private void PrintData(string dataType, int[] values)
        {
            Console.WriteLine($"{dataType} - {string.Join(",", values.Select(x => x.ToString()).ToArray())}");
        }

        private void PrintData(string dataType, object value)
        {
            Console.WriteLine($"{dataType} - {value}");
        }

        public void Start()
        {
            ProcessChain();
        }
    }
}
