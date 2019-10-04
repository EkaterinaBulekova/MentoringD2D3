using System;
using System.Text;
using System.Threading;

namespace AsyncCalculator
{
    public class AsyncSumStarter
    {
        private CancellationTokenSource tokenSource;
        private AsyncSum asyncSum;
        private string userInput;

        public AsyncSumStarter()
        {
            tokenSource = new CancellationTokenSource();
            asyncSum = new AsyncSum();
        }

        public void StartSumCount()
        {
            while (userInput != null)
            {
                TryGetCountIfNumber();
                WaitNewInputToCancel();
            }
        }

        private void TryGetCountIfNumber()
        {
            long number;
            if (long.TryParse(userInput, out number))
                asyncSum.GetCalculator(number, tokenSource.Token);
            else
                Console.WriteLine($"{userInput} isn't number.");
        }

        public void ReadUserInput()
        {
            userInput = ReadLineWithCancel();
        }

        private void WaitNewInputToCancel()
        {
            ReadUserInput();
            SendCancelation();
        }

        private void SendCancelation()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
            tokenSource = new CancellationTokenSource();
        }

        private string ReadLineWithCancel()
        {
            string result = null;
            StringBuilder buffer = new StringBuilder();

            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                Console.Write(info.KeyChar);
                buffer.Append(info.KeyChar);
                info = Console.ReadKey(true);
            }

            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }
            Console.WriteLine();

            return result;
        }
    }

}

