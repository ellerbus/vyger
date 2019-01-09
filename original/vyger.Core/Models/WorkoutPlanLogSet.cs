namespace vyger.Core.Models
{
    public class WorkoutPlanLogSet : WorkoutSet
    {
        #region Constructors

        public WorkoutPlanLogSet() : base() { }

        public WorkoutPlanLogSet(string workoutPlan) : base(workoutPlan)
        {
        }

        #endregion

        #region Properties

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
