using System.Collections.Generic;
using System.Linq;

namespace vyger.Core.Models
{
    public class WorkoutPlanLogSetCollection : List<WorkoutPlanLogSet>
    {
        #region Constructors

        public WorkoutPlanLogSetCollection()
        {
        }

        public WorkoutPlanLogSetCollection(string workoutPlan)
        {
            IEnumerable<WorkoutPlanLogSet> sets = workoutPlan
                .Replace(" ", "")
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new WorkoutPlanLogSet(x));

            AddRange(sets);
        }

        #endregion

        #region Methods

        public static string Format(string workoutPlan)
        {
            WorkoutPlanLogSetCollection sets = new WorkoutPlanLogSetCollection(workoutPlan);

            return sets.Display;
        }

        public override string ToString()
        {
            return string.Join(", ", this.Select(x => x.Display));
        }

        #endregion

        #region Properties

        public string Display
        {
            get { return ToString(); }
        }

        #endregion
    }
}