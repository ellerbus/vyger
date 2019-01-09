using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
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

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="week">1-9 or 0 for all</param>
        ///// <param name="day">1-7 or 0 for all</param>
        ///// <param name="exercise">ID or null for all</param>
        ///// <returns></returns>
        //public IEnumerable<RoutineExercise> Filter(int week, int day, string exercise)
        //{
        //    return this
        //        .Where(x => week == 0 || x.Week == week)
        //        .Where(x => day == 0 || x.Day == day)
        //        .Where(x => exercise.IsNullOrEmpty() || x.exercise.IsSameAs(exercise))
        //        .OrderBy(x => x.Week)
        //        .ThenBy(x => x.Day)
        //        .ThenBy(x => x.SequenceNumber)
        //        .ThenBy(x => x.Exercise?.DetailName);
        //}

        //public void Add(int dayId, string exerciseId, string workoutRoutine)
        //{
        //    workoutRoutine = RoutineSetCollection.Format(workoutRoutine);

        //    Exercise exercise = Routine.AllExercises.GetByPrimaryKey(exerciseId);

        //    List<RoutineExercise> routineExercises = new List<RoutineExercise>();

        //    for (int w = 0; w < Routine.Weeks; w++)
        //    {
        //        RoutineExercise routineExercise = new RoutineExercise()
        //        {
        //            Routine = Routine,
        //            ExerciseId = exercise.Id,
        //            WeekId = w + 1,
        //            DayId = dayId,
        //            Routine = workoutRoutine,
        //            SequenceNumber = 99
        //        };

        //        Add(routineExercise);
        //    }
        //}

        //public void DeleteRoutineExercise(int dayId, string exerciseId)
        //{
        //    IList<RoutineExercise> remove = Filter(0, dayId, exerciseId).ToList();

        //    foreach (RoutineExercise ex in remove)
        //    {
        //        Remove(ex);
        //    }
        //}

        #endregion

        #region Foreign Key Properties

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public Routine Routine { get; private set; }

        ///// <summary>
        ///// Only set if this collection is a subset (or a single week basically)
        ///// &gt 1 means there's a week assigned
        ///// </summary>
        //public int WeekId { get; set; }

        ///// <summary>
        ///// Only set if this collection is a subset (or a single day basically)
        ///// &gt 1 means there's a day assigned
        ///// </summary>
        //public int DayId { get; set; }

        ///// <summary>
        ///// Only set if this collection is a subset (or a single exercise basically)
        ///// &gt 1 means there's a exercise assigned
        ///// </summary>
        //public int ExerciseId { get; set; }

        #endregion
    }
}