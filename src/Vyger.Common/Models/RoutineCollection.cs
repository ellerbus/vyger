using System;
using System.Collections.Generic;
using System.Diagnostics;
using Augment;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class RoutineCollection : SingleKeyCollection<Routine, string>
    {
        #region Constructors

        public RoutineCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public RoutineCollection(IEnumerable<Routine> routines)
            : this()
        {
            if (routines != null)
            {
                foreach (Routine routine in routines)
                {
                    Add(routine);
                }
            }
        }

        #endregion

        #region ToString/DebuggerDisplay

        ///	<summary>
        ///	DebuggerDisplay for this object
        ///	</summary>
        private string DebuggerDisplay
        {
            get { return "Count={0}".FormatArgs(Count); }
        }

        #endregion

        #region Methods

        protected override string GetPrimaryKey(Routine item)
        {
            return item.Id;
        }

        #endregion
    }
}