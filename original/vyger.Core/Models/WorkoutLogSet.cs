namespace vyger.Core.Models
{
    public class WorkoutLogSet : WorkoutSet
    {
        #region Constructors

        public WorkoutLogSet() : base() { }

        public WorkoutLogSet(string workoutLog) : base(workoutLog) { }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public double OneRepMax
        {
            get { return WorkoutCalculator.OneRepMax(Weight, Reps); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override bool ExpandSetsOnDisplay
        {
            get { return true; }
        }

        #endregion
    }
}