using System.Collections.Generic;
using vyger.Core;
using vyger.Core.Models;

namespace vyger.ViewModels
{
    public class ExerciseIndexViewModel
    {
        public ExerciseIndexViewModel() { }

        public IList<Exercise> Items { get; set; }

        public ExerciseCategories SelectedCategory { get; set; }

        public ExerciseGroups SelectedGroup { get; set; }
    }
}