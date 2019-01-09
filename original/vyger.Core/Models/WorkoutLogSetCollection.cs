using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vyger.Core.Models
{
    public class WorkoutLogSetCollection : List<WorkoutLogSet>
    {
        #region Constructors

        public WorkoutLogSetCollection()
        {
        }

        public WorkoutLogSetCollection(string workoutLog)
        {
            IEnumerable<WorkoutLogSet> sets = workoutLog
                .Replace(" ", "")
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new WorkoutLogSet(x));

            AddRange(sets);
        }

        #endregion

        #region Methods

        public static string Format(string workout)
        {
            WorkoutLogSetCollection sets = new WorkoutLogSetCollection(workout);

            return sets.Display;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (WorkoutLogSet set in this)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(set.Display);
            }

            return sb.ToString();
        }

        public WorkoutLogSet GetOneRepMax()
        {
            WorkoutLogSet set = null;

            foreach (WorkoutLogSet s in this)
            {
                if (set == null || s.OneRepMax > set.OneRepMax)
                {
                    set = s;
                }
            }

            return set;
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