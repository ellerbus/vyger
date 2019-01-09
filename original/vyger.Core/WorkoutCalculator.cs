using System;
using System.Configuration;
using Augment;

namespace vyger.Core
{
    public static class WorkoutCalculator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static double Round(double weight, double increment = 5)
        {
            return Math.Round(weight / increment) * increment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>https://www.exrx.net/Calculators/OneRepMax</remarks>
        public static double OneRepMax(double weight, int reps)
        {
            return weight / (1.0278 - 0.0278 * reps);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>https://www.exrx.net/Calculators/OneRepMax</remarks>
        public static double Prediction(double oneRepMax, int reps)
        {
            return oneRepMax * (1.0278 - 0.0278 * reps);
        }
    }
}
