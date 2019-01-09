using System.ComponentModel.DataAnnotations;
using Augment;

namespace vyger.Core
{
    static class ExerciseCateogryExtensions
    {
        public static string GetIdentifier(this ExerciseCategories category)
        {
            string id = category.ToString().Left(2).ToUpper();

            return id;
        }
    }

    public enum ExerciseCategories
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "(exercise category)")]
        None,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Barbell")]
        Barbell,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Dumbbell")]
        Dumbbell,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Hammer")]
        Hammer,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Machine")]
        Machine,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Body")]
        Body
    }
}
