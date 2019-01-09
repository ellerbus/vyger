using System.ComponentModel.DataAnnotations;
using Augment;

namespace Vyger.Common.Models
{
    public partial class Member
    {
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
                string pk = "[" + Email + "]";

                return $"{GetType().Name}, pk={pk}";
            }
        }

        #endregion

        #region  Properties

        /// <summary>
        /// Property for id
        /// </summary>
        [Display(Name = "Id", Prompt = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Property for email
        /// </summary>
        [Display(Name = "Email", Prompt = "Email")]
        public string Email
        {
            get { return _email.AssertNotNull().ToLower(); }
            set { _email = value.AssertNotNull().ToLower(); }
        }

        private string _email;

        /// <summary>
        /// Property for name
        /// </summary>
        [Display(Name = "Name", Prompt = "Name")]
        public string Name { get; set; }

        #endregion
    }
}