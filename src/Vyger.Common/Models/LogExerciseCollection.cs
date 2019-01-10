using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Augment;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class LogExerciseCollection : Collection<LogExercise>
    {
        #region Constructors

        public LogExerciseCollection()
        {
        }

        public LogExerciseCollection(IEnumerable<LogExercise> exercises)
        {
            if (exercises != null)
            {
                foreach (LogExercise exercise in exercises)
                {
                    Add(exercise);
                }
            }
        }

        #endregion

        #region ToString/DebuggerDisplay

        ///	<summary>
        ///	DebuggerDisplay for this object
        ///	</summary>
        private string DebuggerDisplay
        {
            get { return "Count={0}".FormatArgs(Count); }
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public LogExercise FilterMax(string id)
        {
            LogExercise max = null;

            foreach (LogExercise log in this.Where(x => x.Id.IsSameAs(id)))
            {
                if (max == null || log.OneRepMax > max.OneRepMax)
                {
                    max = log;
                }
            }

            return max;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LogExercise> FilterForUpdating(DateTime date)
        {
            return this.Where(x => x.Date == date);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="week">1-9</param>
        /// <param name="day">1-7</param>
        /// <returns></returns>
        public IEnumerable<LogExercise> FilterDateRange(DateTime start, DateTime end)
        {
            return this
                .Where(x => x.Date.IsBetween(start, end))
                .OrderBy(x => x.Sequence)
                .ThenBy(x => x.Name);
        }

        #endregion
    }
}