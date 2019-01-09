using System.Diagnostics;
using Newtonsoft.Json;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class RoutineExercise : Exercise
    {
        #region Constructors

        public RoutineExercise()
        {
        }

        public RoutineExercise(int week, int day, Exercise master)
        {
            Week = week;
            Day = day;

            Id = master.Id;
            Group = master.Group;
            Category = master.Category;
            Name = master.Name;
        }

        #endregion

        #region ToString/DebuggerDisplay

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        ///	<summary>
        ///	DebuggerDisplay for this object
        ///	</summary>
        private string DebuggerDisplay
        {
            get
            {
                string id = $"[{Week}, {Day}, {Id}]";

                string nm = $"[{Name}]";

                return $"RoutineExericse, id={id}, nm={nm}";
            }
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("week")]
        public int Week { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("day")]
        public int Day { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("sets")]
        public string[] Sets { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonIgnore()]
        public string WorkoutPattern
        {
            get
            {
                if (Sets != null)
                { return WorkoutSet.Combine(Sets); }
                return "";
            }
            set { Sets = WorkoutSet.Expand(value); }
        }

        /////	<summary>
        /////
        /////	</summary>
        //[JsonProperty("workout-routine")]
        //public string WorkoutRoutine
        //{
        //    get { return _workoutRoutine; }
        //    set
        //    {
        //        _workoutRoutine = WorkoutRoutineSetCollection.Format(value.AssertNotNull());

        //        _sets = null;
        //    }
        //}

        //private string _workoutRoutine;

        #endregion

        //#region Foreign Key Properties

        ///// <summary>
        /////
        ///// </summary>
        ///// <returns></returns>
        //[JsonIgnore]
        //public WorkoutRoutineSetCollection Sets
        //{
        //    get
        //    {
        //        if (_sets == null)
        //        {
        //            _sets = new WorkoutRoutineSetCollection(WorkoutRoutine);
        //        }
        //        return _sets;
        //    }
        //}

        //private WorkoutRoutineSetCollection _sets;

        //#endregion
    }
}