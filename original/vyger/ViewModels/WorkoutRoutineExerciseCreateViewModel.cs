using System.Collections.Generic;
using vyger.Core;
using vyger.Core.Models;

namespace vyger.ViewModels
{
    public class WorkoutRoutineExerciseCreateViewModel
    {
        public WorkoutRoutine Routine { get; set; }

        public WorkoutRoutineExercise RoutineExercise { get; set; }

        public IList<Exercise> Exercises { get; set; }

        public ExerciseGroups Group { get; set; }

        public ExerciseCategories Category { get; set; }
    }
}