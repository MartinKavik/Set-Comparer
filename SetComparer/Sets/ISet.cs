using System.Collections.Generic;

namespace SetComparer.Sets
{
    public interface ISet<T>
    {
        /// <summary>
        /// Duplicates counter
        /// </summary>
        int DuplicatesCount { get; set; }

        /// <summary>
        /// Set itself
        /// </summary>
        List<T> Set { get; set; }

        /// <summary>
        /// Parses serialized set
        /// </summary>
        /// <param name="set">Serialized set</param>
        /// <returns>List of elements</returns>
        /// <exception cref="System.FormatException">Thrown when the set is not deserializable</exception>
        List<T> Parse(string set);

        /// <summary>
        /// Compares Set with another one
        /// </summary>
        /// <param name="set">Set for compare</param>
        /// <returns>True if sets are duplicates</returns>
        bool IsDuplicate(ISet<T> set);

        /// <summary>
        /// Serializes set
        /// </summary>
        /// <returns>Serialized set</returns>
        string ToString();
    }
}
