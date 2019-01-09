using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vyger.Core;

namespace vyger.Tests.Core
{
    [TestClass]
    public class RegexPatternTests
    {
        private Regex GetRegex(string pattern)
        {
            return new Regex("^" + pattern + "$");
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchWorkoutSet()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.WorkoutSet);

            regex.IsMatch("BW/5").Should().BeTrue();
            regex.IsMatch("BW/5/2").Should().BeTrue();

            regex.IsMatch("180/5").Should().BeTrue();
            regex.IsMatch("180/5/2").Should().BeTrue();

            regex.IsMatch("1RM/5").Should().BeTrue();
            regex.IsMatch("1RM/5/2").Should().BeTrue();

            regex.IsMatch("1RM90%/5").Should().BeTrue();
            regex.IsMatch("1RM90%/5/2").Should().BeTrue();

            regex.IsMatch("1RM-90%/5").Should().BeTrue();
            regex.IsMatch("1RM-90%/5/2").Should().BeTrue();

            regex.IsMatch("1RM102.5%/5").Should().BeTrue();
            regex.IsMatch("1RM102.5%/5/2").Should().BeTrue();

            regex.IsMatch("1RM-102.5%/5").Should().BeTrue();
            regex.IsMatch("1RM-102.5%/5/2").Should().BeTrue();

            regex.IsMatch("[L]90%/5").Should().BeTrue();
            regex.IsMatch("[L]90%/5/2").Should().BeTrue();

            regex.IsMatch("[1*L]/5").Should().BeTrue();
            regex.IsMatch("[1*L]/5/2").Should().BeTrue();

            regex.IsMatch("[1*L]90%/5").Should().BeTrue();
            regex.IsMatch("[1*L]90%/5/2").Should().BeTrue();

            regex.IsMatch("[1*L]-90%/5").Should().BeTrue();
            regex.IsMatch("[1*L]-90%/5/2").Should().BeTrue();

            regex.IsMatch("[1*L]-102.5%/5").Should().BeTrue();
            regex.IsMatch("[1*L]-102.5%/5/2").Should().BeTrue();

            regex.IsMatch("[123]-102.5%/5").Should().BeTrue();
            regex.IsMatch("[123]-102.5%/5/2").Should().BeTrue();

            regex.IsMatch("1RM-102.5%x1").Should().BeTrue();
            regex.IsMatch("1RM-102.5%x1x1").Should().BeTrue();
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchReference()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.Reference);

            regex.IsMatch("[F]").Should().BeTrue();
            regex.IsMatch("[L]").Should().BeTrue();
            regex.IsMatch("[N]").Should().BeTrue();
            regex.IsMatch("[P]").Should().BeTrue();
            regex.IsMatch("[1]").Should().BeTrue();

            regex.IsMatch("[*FL]").Should().BeTrue();
            regex.IsMatch("[*LL]").Should().BeTrue();
            regex.IsMatch("[*NL]").Should().BeTrue();
            regex.IsMatch("[*PL]").Should().BeTrue();
            regex.IsMatch("[*1L]").Should().BeTrue();
            regex.IsMatch("[1111]").Should().BeFalse();

            regex.IsMatch("[F*L]").Should().BeTrue();
            regex.IsMatch("[L*L]").Should().BeTrue();
            regex.IsMatch("[N*L]").Should().BeTrue();
            regex.IsMatch("[P*L]").Should().BeTrue();
            regex.IsMatch("[1*L]").Should().BeTrue();
            regex.IsMatch("[11*L]").Should().BeFalse();

            regex.IsMatch("[FFL]").Should().BeTrue();
            regex.IsMatch("[LLL]").Should().BeTrue();
            regex.IsMatch("[NNL]").Should().BeTrue();
            regex.IsMatch("[PPL]").Should().BeTrue();
            regex.IsMatch("[11L]").Should().BeTrue();
            regex.IsMatch("[111L]").Should().BeFalse();
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchRepMax()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.RepMax);

            regex.IsMatch("1RM").Should().BeTrue();
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchPercent()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.Percent);

            regex.IsMatch("90%").Should().BeTrue();

            regex.IsMatch("102.5%").Should().BeTrue();
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchBodyWeight()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.BodyWeight);

            regex.IsMatch("BW").Should().BeTrue();
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchStaticWeight()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.StaticWeight);

            regex.IsMatch("5").Should().BeTrue();

            regex.IsMatch("35").Should().BeTrue();

            regex.IsMatch("235").Should().BeTrue();

            regex.IsMatch("2359").Should().BeTrue();

            regex.IsMatch("23599").Should().BeFalse();
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchReps()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.Reps);

            regex.IsMatch("/5").Should().BeTrue();

            regex.IsMatch("/99").Should().BeTrue();

            regex.IsMatch("/999").Should().BeFalse();
        }

        [TestMethod]
        public void Constants_RegexPatterns_Should_MatchSets()
        {
            var regex = GetRegex(Constants.Regex.WorkoutPatterns.Sets);

            regex.IsMatch("/5").Should().BeTrue();

            regex.IsMatch("/99").Should().BeFalse();

            regex.IsMatch("/999").Should().BeFalse();
        }
    }
}
