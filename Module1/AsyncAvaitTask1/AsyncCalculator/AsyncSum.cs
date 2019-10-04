using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCalculator
{
    public class AsyncSum
    {
        public Action<long, CancellationToken> GetCalculator =>
            async (input, token) =>
            {
                var result = await StartCalculateAsync(input, token);
                if (!string.IsNullOrEmpty(result)) Console.WriteLine(result);
            };

        public async Task<string> StartCalculateAsync(long userNumber, CancellationToken token)
        {
            try
            {
                return await Task<string>.Factory.StartNew(() => GetCalculationResult(userNumber, token), token);
            }
            catch
            {
                return "";
            }
        }

        public string GetCalculationResult(long userNumber, CancellationToken token)
        {
            return string.Concat($"Number = {userNumber} | Sum = {CalculateSum(userNumber, token)}");
        }

        public long CalculateSum(long maxNumber, CancellationToken token)
        {
            long result = 0;
            Console.WriteLine($"Start new {maxNumber}");

            for (int i = 0; i <= maxNumber; i++)
            {
                token.ThrowIfCancellationRequested();
                result += i;
            }
            return result;
        }
    }
}
