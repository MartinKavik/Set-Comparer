using SetComparer.Common;
using S = SetComparer.Sets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SetComparer.Comparers
{
    public class SetComparer<TSet, TType> where TSet : S.ISet<TType>, new()
    {
        // unique sets
        protected List<S.ISet<TType>> sets;
        // invalid inputs
        protected List<ReportItem> report;

        public SetComparer()
        {
            sets = new List<S.ISet<TType>>();
            report = new List<ReportItem>();
        }        

        /// <summary>
        /// Add set if it is unique among sets added before
        /// </summary>
        /// <param name="set">Serialized set</param>
        /// <returns>True if the set has been added</returns>
        public bool AddUniqueSet(string set)
        {
            S.ISet<TType> newSet = new TSet();
            try
            {
                newSet.Set = newSet.Parse(set);
                // call overloaded AddUniqueSet
                return AddUniqueSet(newSet);
            }
            catch(FormatException e)
            {
                report.Add((ReportItem)e.Data["ReportItem"]);
                return false;
            }
        }

        /// <summary>
        /// Add set if it is unique among sets added before
        /// </summary>
        /// <param name="set">Set</param>
        /// <returns>True if the set has been added</returns>
        public bool AddUniqueSet(S.ISet<TType> set)
        {
            foreach (S.ISet<TType> savedSet in sets)
            {
                if(savedSet.IsDuplicate(set))
                {
                    // increment duplicates counter in the unique set
                    savedSet.DuplicatesCount++;
                    // the set is duplicate, do not add
                    return false;
                }
            }
            sets.Add(set);
            return true;
        }


        #region GET methods
        /// <summary>
        /// Get number of duplicates
        /// </summary>
        /// <returns>Number of duplicates</returns>
        public int GetDuplicatesCount()
        {
            int dupCount = 0;
            // sum from unique sets
            foreach (S.ISet<TType> set in sets)
                dupCount += set.DuplicatesCount;
            return dupCount;
        }

        /// <summary>
        /// Get number of non-duplicates
        /// </summary>
        /// <returns>Number of uniques</returns>
        public int GetUniquesCount()
        {
            return sets.Count;
        }

        /// <summary>
        /// Get the most frequent duplicate
        /// </summary>
        /// <returns>The most frequent duplicate</returns>
        public S.ISet<TType> GetMostFrequentSet()
        {
            // init for finding the maximum duplicates count
            int maxDupCount = -1;
            S.ISet<TType> mostFrequent = null;

            foreach (S.ISet<TType> set in sets)
            {
                if(set.DuplicatesCount > maxDupCount)
                {
                    maxDupCount = set.DuplicatesCount;
                    mostFrequent = set;
                }
            }
            return mostFrequent;
        }

        /// <summary>
        /// Serializes report to XML format and writes result to the provided <see cref="TextWriter"/>
        /// </summary>
        /// <param name="xml">Stream for output</param>
        public void GetInvalidInputsReport(TextWriter xml)
        {
            XmlSerializer xs = new XmlSerializer(report.GetType(), new XmlRootAttribute("Report"));
            xs.Serialize(xml, report);            
        }
        #endregion
    }
}
