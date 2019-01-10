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
    public class CycleCollection : SingleKeyCollection<Cycle, string>
    {
        #region Constructors

        public CycleCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public CycleCollection(IEnumerable<Cycle> cycles)
            : this()
        {
            if (cycles != null)
            {
                foreach (Cycle cycle in cycles)
                {
                    Add(cycle);
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

        protected override string GetPrimaryKey(Cycle item)
        {
            return item.Id;
        }

        #endregion
    }
}