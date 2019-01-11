using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Augment;
using Newtonsoft.Json;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Cycle
    {
        #region Constructors

        public Cycle()
        {
            Id = Generators.CycleId();

            Inputs = new CycleInputCollection();

            Exercises = new CycleExerciseCollection();
        }

        public Cycle(Routine primary) : this()
        {
            Name = primary.Name;
            Weeks = primary.Weeks;
            Days = primary.Days;

            foreach (RoutineExercise exercise in primary.Exercises)
            {
                Exercises.Add(new CycleExercise(exercise));

                CycleInput input = null;

                if (!Inputs.TryGetByPrimaryKey(exercise.Id, out input))
                {
                    input = new CycleInput(exercise);

                    Inputs.Add(input);
                }

                bool requiresInput = exercise.Sets
                    .Select(x => new WorkoutSet(x))
                    .Where(x => x.Type == WorkoutSetTypes.RepMax)
                    .Any();

                if (requiresInput)
                {
                    input.RequiresInput = true;
                }
            }
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
                string id = $"[{Id}]";

                string nm = $"[{Name}]";

                return "{0}, id={1}, nm={2}".FormatArgs(nameof(Cycle), id, nm);
            }
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Id", Prompt = "Id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Name", Prompt = "Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Weeks", Prompt = "Weeks")]
        [JsonProperty("weeks")]
        public int Weeks { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Days", Prompt = "Days")]
        [JsonProperty("days")]
        public int Days { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        ///	<summary>
        /// week:day
        ///	</summary>
        [Display(Name = "Logged", Prompt = "Logged")]
        [JsonProperty("lastLogged")]
        public string LastLogged { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonIgnore()]
        public int LoggedWeek
        {
            get
            {
                string week = LastLogged.AssertNotNull("0:0").GetLeftOf(":");

                return int.Parse(week);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [JsonIgnore()]
        public int LoggedDay
        {
            get
            {
                string day = LastLogged.AssertNotNull("0:0").GetRightOf(":");

                return int.Parse(day);
            }
        }

        ///	<summary>
        /// week:day
        ///	</summary>
        [JsonProperty("inputs")]
        public CycleInputCollection Inputs { get; set; }

        ///	<summary>
        /// week:day
        ///	</summary>
        [JsonProperty("exercises")]
        public CycleExerciseCollection Exercises { get; set; }

        #endregion
    }
}