using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class CycleExercise : Exercise
    {
        #region Constructors

        public CycleExercise()
        {
        }

        public CycleExercise(RoutineExercise primary)
        {
            Id = primary.Id;
            Group = primary.Group;
            Category = primary.Category;
            Name = primary.Name;

            Week = primary.Week;
            Day = primary.Day;

            Sequence = primary.Sequence;

            Sets = primary.Sets.ToArray();
            Plan = primary.Sets.ToArray();
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

                return $"CycleExericse, id={id}, nm={nm}";
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
        [JsonProperty("plan")]
        public string[] Plan { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonIgnore()]
        public string WorkoutPattern
        {
            get { return WorkoutSet.Combine(Plan, false); }
            set { Plan = WorkoutSet.Expand(value, false); }
        }

        #endregion
    }
}