namespace Lebowski.Synchronization
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    /// <summary>
    /// A state vector, as used in the dOPT algorithm
    /// </summary>
    public struct StateVector : IEquatable<StateVector>, IComparable<StateVector>
    {
        private List<int> values;

        /// <summary>
        /// Creates a state vector with size `size` with all entries set to zero
        /// </summary>
        /// <param name="size">length of the state vector</param>
        public StateVector(int size)
        {
            values = new List<int>(size);
            for(int i = 0; i < size; ++i)
            {
                values.Add(0);
            }
        }
        
        /// <summary>
        /// Gets the number of dimensions that this state vector has.
        /// </summary>
        public int Size
        {
            get { return values.Count; }
        }

        /// <summary>
        /// Compares this StateVector with another instance. Note that this relation
        /// is <i>not</i> symmetric.
        /// </summary>
        /// <param name="other">The StateVector struct to compare with.</param>
        /// <returns></returns>
        public int CompareTo(StateVector other)
        {
            if (Enumerable.SequenceEqual(values, other.values))
            {
                return 0;
            }

            List<int> thisValues = values;
            if (Enumerable.Range(0, values.Count).Any(i => thisValues[i] > other.values[i]))
            {
                return 1;
            }
            return -1;
        }

        /// <summary>
        /// Retrieves the
        /// <param name="index">the number of the dimension to access.
        /// Requires: 0 &lt;= index &lt; Size
        /// </param>
        /// </summary>
        public int this[int index]
        {
            get { return values[index]; }
            set
            {
                values = new List<int>(values);
                values[index] = value;
            }
        }

        /// <summary>
        /// Converts the multidimensional value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[" + string.Join(" ", values.Select(i => i.ToString()).ToArray()) + "]";
        }

        #region Comparison operators
        public static bool operator < (StateVector a, StateVector b)
        {
            return a.CompareTo(b) == -1;
        }

        public static bool operator > (StateVector a, StateVector b)
        {
            return a.CompareTo(b) == 1;
        }

        public static bool operator == (StateVector a, StateVector b)
        {
            return a.CompareTo(b) == 0;
        }
        #endregion

        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            if (obj is StateVector)
                return Equals((StateVector)obj); // use Equals method below
            else
                return false;
        }

        public bool Equals(StateVector other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            int result = values.Count.GetHashCode();
            foreach (int value in values)
            {
                result ^= value.GetHashCode();
            }
            return result;
        }

        public static bool operator != (StateVector left, StateVector right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
}