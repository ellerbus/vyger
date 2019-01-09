using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Serialization;
using Augment;

namespace vyger.Core.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [XmlType("exercise")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Exercise
    {
        #region Constructors

        public Exercise()
        {
        }

        public Exercise(Exercise exercise)
        {
            Id = exercise.Id;
            Name = exercise.Name;
            Group = exercise.Group;
            Category = exercise.Category;
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

                return "{0}, id={1}, nm={2}".FormatArgs(nameof(Exercise), id, nm);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///	Overlay all properties (except primary key, audits)
        /// </summary>
        public void OverlayWith(Exercise other)
        {
            Name = other.Name;
        }

        #endregion

        #region Properties

        ///	<summary>
        ///	GROUP(2)-CATEGORY(1)-EXERCISENAME(3)
        ///	</summary>
        [Key]
        [DisplayName("ID")]
        [XmlAttribute("id")]
        public string Id
        {
            get { return _id; }
            set { _id = value.AssertNotNull().ToUpper(); }
        }

        private string _id;

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public string DetailName
        {
            get { return $"{Group} - {Category} - {Name}"; }
        }

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Category")]
        [XmlAttribute("category")]
        public ExerciseCategories Category { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Group")]
        [XmlAttribute("group")]
        public ExerciseGroups Group { get; set; }

        #endregion
    }
}