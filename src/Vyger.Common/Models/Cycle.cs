using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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

        #endregion
    }
}