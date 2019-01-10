using System.Diagnostics;
using System.Security.Cryptography;

namespace Vyger.Common
{
    public static class Generators
    {
        #region Members

        private static RNGCryptoServiceProvider _crypto = new RNGCryptoServiceProvider();

        private const string _alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";

        //private static long _lastTick;

        private static readonly Stopwatch _stopwatch = new Stopwatch();

        private static object _lock = new object();

        #endregion

        #region Methods

        public static string ExerciseId()
        {
            return RandomId("x", 2);
        }

        public static string RoutineId()
        {
            return RandomId("r", 2);
        }

        public static string CycleId()
        {
            return RandomId("c", 3);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private static string RandomId(string prefix, int size)
        {
            lock (_lock)
            {
                byte[] data = new byte[size];

                _crypto.GetNonZeroBytes(data);

                char[] chars = new char[size];

                for (int i = 0; i < size; i++)
                {
                    chars[i] = _alphabet[data[i] % (_alphabet.Length - 1)];
                }

                return new string(chars);
            }
        }

        ///// <summary>
        ///// Returns a sequential unique id 12 characters in length
        ///// (not intended for distributed ID generation)
        ///// </summary>
        ///// <returns></returns>
        //public static string Next()
        //{
        //    lock (_lock)
        //    {
        //        if (DateTime.UtcNow.Ticks > _lastTick)
        //        {
        //            _stopwatch.Restart();

        //            _lastTick = DateTime.UtcNow.Ticks;
        //        }

        //        _lastTick += _stopwatch.ElapsedTicks;

        //        return Encode(_lastTick);
        //    }
        //}

        ///// <summary>
        ///// Encode the given number into a Base36 string
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //private static string Encode(long input)
        //{
        //    if (input < 0)
        //    {
        //        throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");
        //    }

        //    Stack<char> result = new Stack<char>();

        //    while (input != 0)
        //    {
        //        int index = (int)(input % _alphabet.Length);

        //        result.Push(_alphabet[index]);

        //        input /= _alphabet.Length;
        //    }

        //    return new string(result.ToArray());
        //}

        #endregion
    }
}