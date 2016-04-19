using SetComparer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SetComparer.Sets
{
    public class IntSet : SetBase<int>
    {
        public IntSet(List<int> set) : base(set) { }
        public IntSet(string set) : base(set) { }  
        public IntSet() { }

        public override List<int> Parse(string set)
        {
            // only comma-separated integers
            Regex inputPattern = new Regex("^([0-9]+,)*[0-9]+$");
            if (set == null || !inputPattern.IsMatch(set))
            {
                // throw FormatException with ReportItem for logging
                ThrowAndLogFormatException(set);
            }
            // parsing itself
            return set.Split(',').Select(int.Parse).ToList();
        }
    }
}
