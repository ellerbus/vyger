namespace vyger.Core.Models
{
    public class WorkoutRoutineSet : WorkoutSet
    {
        #region Constructors

        public WorkoutRoutineSet() : base() { }

        public WorkoutRoutineSet(string workoutRoutine) : base(workoutRoutine)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        protected override bool ExpandSetsOnDisplay
        {
            get { return false; }
        }

        #endregion
    }
}
