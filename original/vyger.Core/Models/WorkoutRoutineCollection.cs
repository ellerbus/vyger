using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Augment;

namespace vyger.Core.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [XmlRoot("workout-routines")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutRoutineCollection : SingleKeyCollection<WorkoutRoutine, string>
    {
        #region Constructors

        public WorkoutRoutineCollection()
        {
        }

        public WorkoutRoutineCollection(ExerciseCollection exercises, IEnumerable<WorkoutRoutine> routines)
            : this()
        {
            Exercises = exercises;

            AddRange(routines);
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

        protected override string GetPrimaryKey(WorkoutRoutine item)
        {
            return item.Id;
        }

        protected override void InsertItem(int index, WorkoutRoutine item)
        {
            base.InsertItem(index, item);

            UpdateReferences(item);
        }

        protected override void SetItem(int index, WorkoutRoutine item)
        {
            base.SetItem(index, item);

            UpdateReferences(item);
        }

        private void UpdateReferences(WorkoutRoutine item)
        {
            if (Exercises != null)
            {
                item.AllExercises = Exercises;
            }
        }

        public void AddRange(IEnumerable<WorkoutRoutine> routines)
        {
            if (routines != null)
            {
                foreach (WorkoutRoutine routine in routines)
                {
                    Add(routine);
                }
            }
        }

        #endregion

        #region Foreign Keys

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public ExerciseCollection Exercises { get; set; }

        #endregion
    }
}