using System;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class LogExercise : Exercise
    {
        #region Constructors

        public LogExercise()
        {
        }

        public LogExercise(DateTime date, Exercise primary)
        {
            Date = date;

            Id = primary.Id;
            Group = primary.Group;
            Category = primary.Category;
            Name = primary.Name;
        }

        public LogExercise(DateTime date, CycleExercise primary)
            : this(date, primary as Exercise)
        {
            Sequence = primary.Sequence;

            Sets = primary.Sets.ToArray();
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
                string id = $"[{Date:yyyy-MM-dd}]";

                string nm = $"[{Name}]";

                return $"LogExercise, id={id}, nm={nm}";
            }
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("ymd")]
        [JsonConverter(typeof(YmdDateConverter))]
        public DateTime Date { get; set; }

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
        [JsonProperty("evaluation")]
        public LogCycleEvaluation Evaluation { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonIgnore()]
        public double OneRepMax
        {
            get
            {
                if (Sets != null && Sets.Length > 0)
                {
                    return Sets.Select(x => new WorkoutSet(x).OneRepMax).Max();
                }

                return 0;
            }
        }

        ///	<summary>
        ///
        ///	</summary>
        [JsonIgnore()]
        public string WorkoutPattern
        {
            get { return WorkoutSet.Combine(Sets, false); }
            set { Sets = WorkoutSet.Expand(value, false); }
        }

        #endregion
    }
}