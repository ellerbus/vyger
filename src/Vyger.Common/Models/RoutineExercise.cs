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

        public RoutineExercise(int week, int day, Exercise primary)
        {
            Week = week;
            Day = day;

            Id = primary.Id;
            Group = primary.Group;
            Category = primary.Category;
            Name = primary.Name;
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
            get { return WorkoutSet.Combine(Sets, true); }
            set { Sets = WorkoutSet.Expand(value, true); }
        }

        #endregion
    }
}