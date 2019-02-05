using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Augment;

namespace Vyger.Common.Models
{
    public class WorkoutSet
    {
        #region Constructors

        public WorkoutSet(string set)
        {
            Reference = "L";
            Weight = 0;
            RepMax = 1;
            Percent = 100;
            Reps = 1;
            Repeat = 1;

            Parse(set.Trim());
        }

        #endregion

        #region Methods

        public static string Combine(string[] sets, bool collapseRepeats)
        {
            if (sets == null)
            {
                return "";
            }

            return sets.SelectMany(x => new WorkoutSet(x).Format(collapseRepeats)).Join(", ");
        }

        public static string[] Expand(string workoutPattern, bool collapseRepeats)
        {
            return workoutPattern
                .Split(new[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(x => new WorkoutSet(x).Format(collapseRepeats))
                .ToArray();
        }

        private void Parse(string set)
        {
            string[] parts = set.Split(new[] { 'x', 'X', '/' }, StringSplitOptions.RemoveEmptyEntries);

            LoadWeight(parts);
            LoadReps(parts);
            LoadSets(parts);
        }

        private void LoadWeight(string[] parts)
        {
            if (parts.Length > 0)
            {
                string word = parts[0];

                if (Regex.IsMatch(word, "^[0-9]+$"))
                {
                    LoadStaticWeight(word);
                }
                else if (Regex.IsMatch(word, "^BW$"))
                {
                    Type = WorkoutSetTypes.BodyWeight;
                }
                else if (Regex.IsMatch(word, "^[0-9]RM.*$"))
                {
                    LoadRepMax(word);
                }
                else if (Regex.IsMatch(word, @"^\[[0-9L]\].*$"))
                {
                    LoadReference(word);
                }
                else if (Regex.IsMatch(word, @"^=[0-9L].*$"))
                {
                    LoadReference(word);
                }
            }
        }

        private void LoadStaticWeight(string word)
        {
            Type = WorkoutSetTypes.Static;

            Weight = int.Parse(word);
        }

        private void LoadRepMax(string word)
        {
            Type = WorkoutSetTypes.RepMax;

            string[] items = word.Split(new[] { "RM" }, StringSplitOptions.RemoveEmptyEntries);

            RepMax = int.Parse(items[0]);

            if (items.Length > 1)
            {
                LoadPercent(items[1]);
            }
        }

        private void LoadReference(string word)
        {
            Type = WorkoutSetTypes.Reference;

            string[] items = word.Split(new[] { '-', '*' }, StringSplitOptions.RemoveEmptyEntries);

            if (items[0].Contains("[") && items[0].Contains("]"))
            {
                int pos = items[0].IndexOf(']');

                Reference = items[0].Substring(1, pos - 1);
            }
            else
            {
                Reference = items[0].Substring(1);
            }

            if (items.Length > 1)
            {
                LoadPercent(items[1]);
            }
        }

        private void LoadPercent(string word)
        {
            if (Regex.IsMatch(word, @"^[\-\*]?[0-9]+(.[0-9]+)?%$", RegexOptions.Compiled))
            {
                string p = word.Replace("*", "").Replace("-", "").Replace("%", "");

                Percent = int.Parse(p);
            }
        }

        private void LoadReps(string[] parts)
        {
            if (parts.Length > 1)
            {
                Reps = int.Parse(parts[1]);
            }
            else
            {
                Reps = 1;
            }
        }

        private void LoadSets(string[] parts)
        {
            if (parts.Length > 2)
            {
                Repeat = int.Parse(parts[2]);
            }
            else
            {
                Repeat = 1;
            }
        }

        public IEnumerable<string> Format(bool collapseRepeats)
        {
            List<string> patterns = new List<string>();

            switch (Type)
            {
                case WorkoutSetTypes.BodyWeight:
                    patterns.Add("BW");
                    break;

                case WorkoutSetTypes.Static:
                    patterns.Add($"{Weight:0}");
                    break;

                case WorkoutSetTypes.RepMax:
                    patterns.Add($"{RepMax:0}RM");
                    break;

                case WorkoutSetTypes.Reference:
                    patterns.Add("=" + Reference);
                    break;
            }

            switch (Type)
            {
                case WorkoutSetTypes.RepMax:
                case WorkoutSetTypes.Reference:
                    if (Percent > 0)
                    {
                        string p = $"{Percent:0.00}".Replace(".00", "");

                        if (p != "100")
                        {
                            patterns.Add($"*{p}%");
                        }
                    }
                    break;
            }

            patterns.Add("x" + Reps);

            if (collapseRepeats)
            {
                if (Repeat > 1)
                {
                    patterns.Add("x" + Repeat);
                }

                yield return patterns.Join("");
            }
            else
            {
                string temp = patterns.Join("");

                for (int i = 0; i < Repeat; i++)
                {
                    yield return temp;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public WorkoutSetTypes Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int RepMax { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Percent { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Reps { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double OneRepMax
        {
            get
            {
                if (Type == WorkoutSetTypes.Static)
                {
                    return WorkoutCalculator.OneRepMax(Weight, Reps);
                }

                return 0;
            }
        }

        #endregion
    }
}