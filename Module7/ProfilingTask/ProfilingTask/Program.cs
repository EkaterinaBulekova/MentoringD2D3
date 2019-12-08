using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ProfilingTask
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] salt = GetSalt(isConst: true);

            string passwordText = "MyPassWord567  _";
            Console.WriteLine("passwordText: " + passwordText);

            Run(GeneratePasswordHashUsingSalt, "GeneratePasswordHashUsingSalt", passwordText, salt);
            /*
            Следует оптимизировать System.Security.Cryptography.DeriveBytes.GetBytes().
            Загружен декомпилированный Rfc2898DeriveBytes и включен в прoект как Rfc2898DeriveBytesLocal.
            */

            Run(GeneratePasswordHashUsingSalt_Rfc2898DeriveBytes_Local, "GeneratePasswordHashUsingSalt_Rfc2898DeriveBytes_Local", passwordText, salt);
            /*
            Оптимизируем Rfc2898DeriveBytes.GetBytes() в локальной версии для этого изменяем метод Func.
            */

            Console.ReadKey();
        }



        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {

            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }


        public static string GeneratePasswordHashUsingSalt_Rfc2898DeriveBytes_Local(string passwordText, byte[] salt)
        {

            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytesLocal(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytesLocal(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);

            return passwordHash;
        }


        private static void Run(Func<string, byte[], string> gererateHash, string methodName, string passwordText, byte[] salt)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string hashedPass = gererateHash(passwordText, salt);
            sw.Stop();

            Console.WriteLine($"{methodName}: {sw.ElapsedMilliseconds}");
        }

        private static byte[] GetSalt(bool isConst)
        {
            byte[] saltConst = new byte[]
            {
                205,
                156,
                81,
                32,
                236,
                208,
                213,
                76,
                140,
                69,
                7,
                239,
                202,
                150,
                247,
                115,
            };

            if (isConst)
            {
                return saltConst;
            }
            byte[] saltGenerated = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(saltGenerated);
            }
            return saltGenerated;
        }
    }
}

