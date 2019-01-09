using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vyger.Common;
using Vyger.Common.Models;

namespace Vyger.Tests.Common.Models
{
    [TestClass]
    public class WorkoutSetTests
    {
        [TestMethod]
        public void WorkoutSet_Should_Construct()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("");
            //  assert
            subject.Reference.Should().Be("L");
            subject.Weight.Should().Be(0);
            subject.RepMax.Should().Be(1);
            subject.Percent.Should().Be(100);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BodyWeight()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("BW");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.BodyWeight);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("BWx1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BodyWeight_WithReps()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("BWx5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.BodyWeight);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("BWx5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BodyWeight_WithRepeat()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("BWx5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.BodyWeight);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("BWx5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_StaticWeight()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("123");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Static);
            subject.Weight.Should().Be(123);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("123x1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_StaticWeight_WithReps()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("123x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Static);
            subject.Weight.Should().Be(123);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("123x5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_StaticWeight_WithRepeats()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("123x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Static);
            subject.Weight.Should().Be(123);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("123x5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=Lx1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithReps()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=Lx5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithRepeats()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=Lx5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]*95%");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_AndReps()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]*95%x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_AndRepeats()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]*95%x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=L*95%x5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_UsingDashes()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]-95%");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_AndReps_UsingDashes()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]-95%x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_AndRepeats_UsingDashes()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]-95%x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=L*95%x5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_UsingMutiplier()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]*95%");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_AndReps_UsingMutiplier()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]*95%x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_BracketReference_WithPercents_AndRepeats_UsingMutiplier()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("[L]*95%x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=L*95%x5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=Lx1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithReps()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=Lx5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=Lx5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithRepeats()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=Lx5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=Lx5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L*95%");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_AndReps()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L*95%x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_AndRepeats()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L*95%x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=L*95%x5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_UsingDashes()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L-95%");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_AndReps_UsingDashes()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L-95%x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_AndRepeats_UsingDashes()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L-95%x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=L*95%x5x3");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_UsingMutiplier()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L*95%");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(1);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x1");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_AndReps_UsingMutiplier()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L*95%x5");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(1);
            subject.Display.Should().Be("=L*95%x5");
        }

        [TestMethod]
        public void WorkoutSet_Should_Recognize_AtSignReference_WithPercents_AndRepeats_UsingMutiplier()
        {
            //  arrange
            //  act
            var subject = new WorkoutSet("=L*95%x5x3");
            //  assert
            subject.Type.Should().Be(WorkoutSetTypes.Reference);
            subject.Reference.Should().Be("L");
            subject.Percent.Should().Be(95);
            subject.Reps.Should().Be(5);
            subject.Repeat.Should().Be(3);
            subject.Display.Should().Be("=L*95%x5x3");
        }
    }
}