using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Augment;

namespace vyger.Core.Models
{
    public abstract class WorkoutSet
    {
        #region Members

        private static readonly Regex _regex = new Regex(Constants.Regex.WorkoutPatterns.WorkoutSet, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

        #region Constructors

        public WorkoutSet()
        {
            Percent = 1.0;
            Reps = 1;
            Sets = 1;
        }

        public WorkoutSet(string workoutSet) : this()
        {
            Match match = _regex.Match(workoutSet);

            foreach (Group group in match.Groups.Cast<Group>().Where(x => x.Success))
            {
                switch (group.Name)
                {
                    case "body":
                        Weight = -1;
                        break;
                    case "weight":
                        Weight = int.Parse(group.Value);
                        break;
                    case "repmax":
                        Weight = 0;
                        RepMax = int.Parse(group.Value.ToUpper().GetLeftOf("RM"));
                        break;
                    case "ref":
                        Reference = group.Value.Substring(1, group.Value.Length - 2);
                        if (Reference.Length == 1)
                        {
                            SetId = ParseIndex(Reference[0]);
                        }
                        else
                        {
                            WeekId = ParseIndex(Reference[0]);
                            DayId = ParseIndex(Reference[1]);
                            SetId = ParseIndex(Reference[2]);
                        }
                        break;
                    case "percent":
                        Percent = double.Parse(group.Value.GetLeftOf("%")) / 100;
                        break;
                    case "reps":
                        Reps = int.Parse(group.Value);
                        break;
                    case "sets":
                        Sets = int.Parse(group.Value);
                        break;
                }
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Display;
        }

        private string CreateDisplay()
        {
            StringBuilder sb = new StringBuilder();

            if (IsReference)
            {
                sb.Append("[").Append(Reference).Append("]");
            }
            else if (IsRepMax)
            {
                sb.Append(RepMax).Append("RM");
            }
            else if (IsBodyWeight)
            {
                sb.Append("BW");
            }
            else if (IsStaticWeight)
            {
                sb.Append(Weight);
            }

            if (Percent != 1.0)
            {
                if (IsRepMax)
                {
                    sb.Append("-");
                }
                sb.Append($"{Percent:0.0%}".Replace(".0", ""));
            }

            sb.Append("x").Append(Reps);

            return sb.ToString();
        }

        private int ParseIndex(char value)
        {
            switch (value)
            {
                //  first
                case 'f':
                case 'F':
                    return Constants.Referencing.First;

                //  last
                case 'l':
                case 'L':
                    return Constants.Referencing.Last;

                //  prev
                case 'p':
                case 'P':
                    return Constants.Referencing.Previous;

                //  next
                case 'n':
                case 'N':
                    return Constants.Referencing.Next;

                //  same
                case '*':
                    return Constants.Referencing.Same;

                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;

                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The static weight (-1 for bodyweight)
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Is Body Weight
        /// </summary>
        public bool IsBodyWeight { get { return Weight == -1; } }

        /// <summary>
        /// Is Static Weight
        /// </summary>
        public bool IsStaticWeight { get { return Weight > 0; } }

        /// <summary>
        /// #RM - RepMax
        /// </summary>
        public int RepMax { get; private set; }

        /// <summary>
        /// Is ORM
        /// </summary>
        public bool IsRepMax { get { return RepMax > 0; } }

        /// <summary>
        /// week-day-set
        /// </summary>
        public string Reference { get; private set; }

        /// <summary>
        /// Is Reference
        /// </summary>
        public bool IsReference { get { return Reference.IsNotEmpty(); } }

        /// <summary>
        /// Reference Week ID
        /// </summary>
        public int WeekId { get; set; }

        /// <summary>
        /// Reference Day ID
        /// </summary>
        public int DayId { get; set; }

        /// <summary>
        /// Reference Set ID
        /// </summary>
        public int SetId { get; set; }

        /// <summary>
        /// Using a Percent (stored as 0.50 for 50%)
        /// </summary>
        public double Percent { get; private set; }

        /// <summary>
        /// Reps
        /// </summary>
        public int Reps { get; protected set; }

        /// <summary>
        /// Sets
        /// </summary>
        public int Sets { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        protected abstract bool ExpandSetsOnDisplay { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Display
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                string display = CreateDisplay();

                sb.Append(display);

                if (ExpandSetsOnDisplay)
                {
                    for (int x = 1; x < Sets; x++)
                    {
                        sb.Append(", ").Append(display);
                    }
                }
                else if (Sets > 1)
                {
                    sb.Append("x").Append(Sets);
                }

                return sb.ToString();
            }
        }

        #endregion
    }
}
