using System;
using System.Linq;
using System.Collections.Generic;

namespace Lebowski
{
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
		
		public int CompareTo(StateVector other)
		{
			if(Enumerable.SequenceEqual(values, other.values))
			{
				return 0;
			}
	
			List<int> thisValues = values;
			if(Enumerable.Range(0, values.Count).Any(i => thisValues[i] > other.values[i]))
			{
				return 1;
			}
			return -1;
		}		
		
		public int this[int index]
		{
			get { return values[index]; }
			set
			{
				values = new List<int>(values);
				values[index] = value;
			}
		}		
		
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
			foreach(int value in values)
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
