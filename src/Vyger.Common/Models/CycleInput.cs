using System.Diagnostics;
using Newtonsoft.Json;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class CycleInput : Exercise
    {
        #region Constructors

        public CycleInput()
        {
        }

        public CycleInput(Exercise primary)
        {
            Id = primary.Id;
            Group = primary.Group;
            Category = primary.Category;
            Name = primary.Name;

            Reps = 1;
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

                return $"CycleInput, id={id}, nm={nm}";
            }
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("weight")]
        public int Weight { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("reps")]
        public int Reps { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("pullback")]
        public int Pullback { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonProperty("requiresInput")]
        public bool RequiresInput { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [JsonIgnore()]
        public double OneRepMax
        {
            get
            {
                return WorkoutCalculator.OneRepMax(Weight, Reps);
            }
        }

        #endregion
    }
}