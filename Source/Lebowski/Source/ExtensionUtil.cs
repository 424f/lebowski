using System;
using System.Collections.Generic;

namespace Lebowski
{
	public static class ExtensionUtil
	{
		public static Type[] FindTypesImplementing(Type type)
		{
			List<Type> result = new List<Type>();
			foreach(Type t in typeof(ExtensionUtil).Assembly.GetTypes())
			{
				if(t.IsClass && t.GetConstructors().Length > 0 && type.IsAssignableFrom(t))
				{
					result.Add(t);
				}
			}
			return result.ToArray();
		}
	}
}
