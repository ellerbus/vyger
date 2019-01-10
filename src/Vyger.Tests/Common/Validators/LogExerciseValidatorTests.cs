using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vyger.Common.Models;
using Vyger.Common.Validators;

namespace Vyger.Tests.Common.Validators
{
    [TestClass]
    public class LogExerciseValidatorTests : BaseValidatorTests<LogExerciseValidator>
    {
        #region Members

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        #endregion
        
        #region Validation Rule Tests
        
        [TestMethod]
        public void LogExerciseValidator_RuleFor_Id()
        {
            var item = new LogExercise();

            item.Id = default(string);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Id, item);
            
            item.Id = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Id, item);
            
            item.Id = new string('0', 3);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Id, item);
            
            item.Id = new string('0', 3 + 1);
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Id, item);
            
        }
        
        
        [TestMethod]
        public void LogExerciseValidator_RuleFor_Name()
        {
            var item = new LogExercise();

            item.Name = default(string);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Name, item);
            
            item.Name = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Name, item);
            
            item.Name = new string('0', 150);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Name, item);
            
            item.Name = new string('0', 150 + 1);
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Name, item);
            
        }
        
        
        [TestMethod]
        public void LogExerciseValidator_RuleFor_Group()
        {
            var item = new LogExercise();

            item.Group = default(string);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Group, item);
            
            item.Group = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Group, item);
            
            item.Group = new string('0', 150);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Group, item);
            
            item.Group = new string('0', 150 + 1);
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Group, item);
            
        }
        
        
        [TestMethod]
        public void LogExerciseValidator_RuleFor_Category()
        {
            var item = new LogExercise();

            item.Category = default(string);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Category, item);
            
            item.Category = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Category, item);
            
            item.Category = new string('0', 150);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Category, item);
            
            item.Category = new string('0', 150 + 1);
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Category, item);
            
        }
        
        
        [TestMethod]
        public void LogExerciseValidator_RuleFor_Date()
        {
            var item = new LogExercise();

            item.Date = default(string);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Date, item);
            
            item.Date = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Date, item);
            
            item.Date = new string('0', 10);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Date, item);
            
            item.Date = new string('0', 10 + 1);
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Date, item);
            
        }
        
        
        [TestMethod]
        public void LogExerciseValidator_RuleFor_Sets()
        {
            var item = new LogExercise();

            item.Sets = default(string);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Sets, item);
            
            item.Sets = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Sets, item);
            
            item.Sets = new string('0', 150);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Sets, item);
            
            item.Sets = new string('0', 150 + 1);
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Sets, item);
            
        }
        
        
        #endregion
    }
}