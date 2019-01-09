using System.Collections.Generic;
using EnsureThat;

namespace vyger.Core
{
    public static class Constants
    {
        #region Roles

        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string ActiveMember = "ActiveMember";
        }

        #endregion

        #region Constants

        public static class Referencing
        {
            public const int First = 10000;
            public const int Previous = -10000;
            public const int Same = 0;
            public const int Next = -99999;
            public const int Last = 99999;
        }

        public static class Regex
        {
            public static class IdPatterns
            {
                /// <summary>
                /// 
                /// </summary>
                public const string Exercise = @"[A-Z]{3}-[A-Z]{2}-[A-Z]{3}";
            }

            /// <summary>
            /// 
            /// </summary>
            public static class WorkoutPatterns
            {
                /// <summary>
                /// Represents the workout routine pattern
                /// </summary>
                public const string WorkoutSets = "^" + SingleWorkoutSet + "(, " + SingleWorkoutSet + ")*$";

                /// <summary>
                /// Represents the workout routine pattern
                /// </summary>
                public const string WorkoutSet = "^" + SingleWorkoutSet + "$";

                /// <summary>
                /// Represents the workout routine pattern
                /// </summary>
                private const string SingleWorkoutSet =
                    "((" + BodyWeight + ")|(" + RepMax + "(-?" + Percent + ")?)|(" + Reference + "(-?" + Percent + ")?)|(" + StaticWeight + "))" +
                    "(" + Reps + ")" +
                    "(" + Sets + ")?";

                /// <summary>
                /// Represents the reps portion of a workout pattern
                /// </summary>
                public const string Reps = @"[xX/](?<reps>[1-9][0-9]?)";

                /// <summary>
                /// Represents the sets portion of a workout pattern
                /// </summary>
                public const string Sets = @"[xX/](?<sets>[1-9])";

                /// <summary>
                /// navigation-week/day/set
                /// </summary>
                public const string Reference = @"(?<ref>\[([FLPN1-9\*][FLPN1-9\*])?[FLPN1-9\*]\])";

                /// <summary>
                /// 
                /// </summary>
                public const string Percent = @"(?<percent>(\d{2,3}(\.\d)?%))";

                /// <summary>
                /// 
                /// </summary>
                public const string StaticWeight = @"(?<weight>(\d{1,4}))";

                /// <summary>
                /// 
                /// </summary>
                public const string BodyWeight = @"(?<body>(BW))";

                /// <summary>
                /// 
                /// </summary>
                public const string RepMax = @"(?<repmax>([0-9]?[0-9]RM))";

                /*
                            const string weight = @"((?<weight>\d+)(?<repmax>RM)?)";

                            const string reference = @"(?<reference>(W(?<week>\d+|\!))?(D(?<day>\d+|\!))?S(?<set>\d+|\!))";

                            string pattern = "(" + weight + "|" + reference + ")" +
                                @"(\*(?<percent>\d+(\.\d+)?)\%?)?" +
                                @"(\s*[X/](?<reps>\d+)(\s*[X/](?<sets>\d+))?)?";*/
            }
        }

        public const string Title = "vyger";

        public const int MinDays = 1;
        public const int MaxDays = 7;

        public const int MinWeeks = 1;
        public const int MaxWeeks = 9;

        public const int MinCycles = 1;
        public const int MaxCycles = 12;

        public const int MinReps = 1;
        public const int MaxReps = 12;

        public const int MinPullback = 0;
        public const int MaxPullback = 50;

        public const int MinWeight = 1;
        public const int MaxWeight = 9999;

        #endregion

        #region Simple Sequencer

        public static string SequenceId(int id, int len = 2)
        {
            Ensure.That(id, nameof(id)).IsGt(0);

            const string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Stack<char> stack = new Stack<char>();

            while (id > 0)
            {
                stack.Push(alphabet[id % alphabet.Length]);

                id /= alphabet.Length;
            }

            return new string(stack.ToArray()).PadLeft(len, '0');
        }

        #endregion
    }
}
