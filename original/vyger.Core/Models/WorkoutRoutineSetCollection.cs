using System.Collections.Generic;
using System.Linq;

namespace vyger.Core.Models
{
    public class WorkoutRoutineSetCollection : List<WorkoutRoutineSet>
    {
        #region Constructors

        public WorkoutRoutineSetCollection()
        {
        }

        public WorkoutRoutineSetCollection(string workoutRoutine)
        {
            IEnumerable<WorkoutRoutineSet> sets = workoutRoutine
                .Replace(" ", "")
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new WorkoutRoutineSet(x));

            AddRange(sets);
        }

        #endregion

        #region Methods

        public static string Format(string workoutRoutine)
        {
            WorkoutRoutineSetCollection sets = new WorkoutRoutineSetCollection(workoutRoutine);

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