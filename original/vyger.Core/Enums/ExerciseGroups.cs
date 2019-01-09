using System.ComponentModel.DataAnnotations;
using Augment;

namespace vyger.Core
{
    static class ExerciseGroupExtensions
    {
        public static string GetIdentifier(this ExerciseGroups group)
        {
            string id = group.ToString().Left(3).ToUpper();

            return id;
        }
    }

    public enum ExerciseGroups
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "(exercise group)")]
        None,
        [Display(Name = "Abs")]
        Abs,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Biceps")]
        Biceps,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Calves")]
        Calves,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Chest")]
        Chest,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Forearms")]
        Forearms,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Legs")]
        Legs,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Lower Back")]
        LowerBack,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Shoulders")]
        Shoulders,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Traps")]
        Traps,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Triceps")]
        Triceps,
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Upper Back")]
        UpperBack,
    }
}
