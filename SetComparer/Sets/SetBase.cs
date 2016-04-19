using SetComparer.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetComparer.Sets
{
    /// <summary>
    /// Base class for SetComparers' Sets
    /// </summary>
    /// <typeparam name="T">The element type of the set</typeparam>
    public abstract class SetBase<T> : ISet<T>
    {         
        protected int duplicatesCount = 0;
        protected List<T> set;


        #region Properties        
        public int DuplicatesCount
        {
            get { return duplicatesCount; }
            set { duplicatesCount = value; }
        }        
        
        public List<T> Set
        {
            get { return set; }
            set { set = value; }
        }
        #endregion


        #region Constructors
        protected SetBase(List<T> set)
        {
            this.set = set;
        }

        protected SetBase(string set)
        {
            this.set = Parse(set);
        }

        protected SetBase()
        {
            this.set = new List<T>();
        }
        #endregion


        #region Protected methods
        protected void ThrowAndLogFormatException(string set)
        {
            // throw FormatException with ReportItem for logging
            if (set == null) set = "null";
            FormatException fe = new FormatException(string.Format("Wrong input: {0}", set));
            ReportItem ri = new ReportItem(set, DateTime.UtcNow);
            fe.Data.Add("ReportItem", ri);
            throw fe;
        }
        #endregion


        #region Public Methods        
        abstract public List<T> Parse(string set);

        public virtual bool IsDuplicate(ISet<T> set)
        {
            /* Two sets are considered duplicate when they contain the same number of items
             * and their items are of the same value no matter what positions they are at.
             *
             * Complexity: O(N), assuming that the Dictionary (hash table) uses O(1) lookups
             */

            List<T> set1 = this.set;
            List<T> set2 = set.Set;

            if (set1.Count != set2.Count)
                return false;

            Dictionary<T, int> dict = new Dictionary<T, int>();

            // get number of occurences for each unique element in set1
            foreach (T item in set1)
            {
                if (dict.ContainsKey(item))
                    dict[item]++;
                else                    
                    dict.Add(item, 1);
            }

            // subtract number of occurences from set2
            foreach (T item in set2)
            {
                if (dict.ContainsKey(item))                    
                    dict[item]--;
                else
                    return false;
            }

            // all dictionary values are 0 if sets are duplicates
            return dict.Values.All(v => v == 0);
        }   
        
        public override string ToString()
        {
            // comma-separated values
            return String.Join<T>(",", set);
        }
        #endregion
    }
}
