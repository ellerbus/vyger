using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vyger.Core.Models;
using vyger.Core.Validators;

namespace vyger.Tests.Core.Validators
{
    [TestClass]
    public class WorkoutRoutineExerciseValidatorTests : MoxyTesting<WorkoutRoutineExerciseValidator>
    {
        [TestMethod]
        public void WorkoutRoutineExerciseValidatord_RuleFor_WorkoutRoutine_Basic()
        {
            var item = new WorkoutRoutineExercise();

            item.WorkoutRoutine = null;
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.WorkoutRoutine, item);
        }

        [TestMethod]
        public void WorkoutRoutineExerciseValidatord_RuleFor_WorkoutRoutine_WithBodyWeight()
        {
            var item = new WorkoutRoutineExercise();

            //
            //  BODY WEIGHT
            //

            item.WorkoutRoutine = "BW/5";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "BW/5/2";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);
        }

        [TestMethod]
        public void WorkoutRoutineExerciseValidatord_RuleFor_WorkoutRoutine_WithStaticWeight()
        {
            var item = new WorkoutRoutineExercise();

            //
            //  STATIC WEIGHT
            //

            item.WorkoutRoutine = "180/5";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "180/5/2";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);
        }

        [TestMethod]
        public void WorkoutRoutineExerciseValidatord_RuleFor_WorkoutRoutine_WithRepMax()
        {
            var item = new WorkoutRoutineExercise();

            //
            //  1RM WEIGHT
            //

            item.WorkoutRoutine = "1RM/5";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "1RM/5/2";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "1RM90%/5";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "1RM90%/5/2";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "1RM102.5%/5";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);

            item.WorkoutRoutine = "1RM102.5%/5/2";
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.WorkoutRoutine, item);
        }
    }
}
