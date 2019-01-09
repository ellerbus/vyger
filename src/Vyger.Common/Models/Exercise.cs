using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Exercise
    {
        #region Constructors

        public Exercise()
        {
            Id = Generators.ExerciseId();
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

                return $"Exercise, id={id}, nm={nm}";
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

        /// <summary>
        ///
        /// </summary>
        [JsonIgnore]
        public string DisplayName
        {
            get { return $"{Group} - {Category} - {Name}"; }
        }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Category", Prompt = "Category")]
        [JsonProperty("category")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExerciseCategories Category { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Group", Prompt = "Group")]
        [JsonProperty("group")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExerciseGroups Group { get; set; }

        #endregion
    }
}