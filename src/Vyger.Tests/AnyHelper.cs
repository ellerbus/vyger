using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Vyger.Common.Models;

namespace Vyger.Tests
{
    public class Any
    {
        #region Value Types

        public static int Integer { get { return It.IsAny<int>(); } }

        public static double Double { get { return It.IsAny<double>(); } }

        public static DateTime DateTime { get { return It.IsAny<DateTime>(); } }

        public static string String { get { return It.IsAny<string>(); } }

        public static string[] StringArray { get { return It.IsAny<string[]>(); } }

        public static IEnumerable<string> StringEnumerable { get { return It.IsAny<IEnumerable<string>>(); } }

        #endregion

        #region Domain Models

        public static Member Member { get { return It.IsAny<Member>(); } }

        #endregion
    }

    public class AnyValidation
    {
        public static ValidationResult Success { get { return new ValidationResult(); } }

        public static ValidationResult Failure { get { return new ValidationResult(new[] { new ValidationFailure("", "Failed!") }); } }

        public static ValidationException FailureException { get { return new ValidationException(new[] { new ValidationFailure("", "Failed!") }); } }
    }
}