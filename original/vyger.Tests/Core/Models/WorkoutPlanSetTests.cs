using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class WorkoutPlanSetTests
    {
        [TestMethod]
        public void WorkoutPlanSet_Constructor_Should_BeStatic()
        {
            // arrange
            var subject = new WorkoutPlanSet("135x5");
            // act
            // assert
            subject.Display.Should().Be("135/5");
        }

        [TestMethod]
        public void WorkoutPlanSet_Constructor_Should_BeStatic_Alternate()
        {
            // arrange
            var subject = new WorkoutPlanSet("135/5");
            // act
            // assert
            subject.Display.Should().Be("135/5");
        }

        [TestMethod]
        public void WorkoutPlanSet_Constructor_Should_BeStatic_WithSets()
        {
            // arrange
            var subject = new WorkoutPlanSet("135x5x2");
            // act
            // assert
            subject.Display.Should().Be("135/5, 135/5");
        }

        [TestMethod]
        public void WorkoutPlanSet_Constructor_Should_BeStatic_WithSets_Alternate()
        {
            // arrange
            var subject = new WorkoutPlanSet("135/5/2");
            // act
            // assert
            subject.Display.Should().Be("135/5, 135/5");
        }
    }
}