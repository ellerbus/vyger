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
    public class CycleInputCollection : SingleKeyCollection<CycleInput, string>
    {
        #region Constructors

        public CycleInputCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public CycleInputCollection(IEnumerable<CycleInput> inputs)
            : this()
        {
            if (inputs != null)
            {
                foreach (CycleInput input in inputs)
                {
                    Add(input);
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

        protected override string GetPrimaryKey(CycleInput item)
        {
            return item.Id;
        }

        #endregion
    }
}