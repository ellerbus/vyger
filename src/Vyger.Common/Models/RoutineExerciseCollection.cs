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
    public class RoutineExerciseCollection : Collection<RoutineExercise>
    {
        #region Constructors

        public RoutineExerciseCollection()
        {
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
        public RoutineExercise FilterForUpdate(RoutineExercise search)
        {
            return FilterForUpdating(search.Day, search.Id).First(x => x.Week == search.Week);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoutineExercise> FilterForUpdating(int day, string id)
        {
            return this.Where(x => x.Day == day && x.Id.IsSameAs(id));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="week">1-9</param>
        /// <param name="day">1-7</param>
        /// <returns></returns>
        public IEnumerable<RoutineExercise> Filter(int week, int day)
        {
            return this
                .Where(x => x.Week == week && x.Day == day)
                .OrderBy(x => x.Sequence)
                .ThenBy(x => x.Name);
        }

        #endregion
    }
}