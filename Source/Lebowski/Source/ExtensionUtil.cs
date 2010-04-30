using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lebowski
{
	public static class ExtensionUtil
	{
		public static Type[] FindTypesImplementing(Type type)
		{
			List<Type> result = new List<Type>();
			foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
			{
				foreach(Type t in assembly.GetTypes())
				{
					if(t.IsClass && t.GetConstructors().Length > 0 && type.IsAssignableFrom(t))
					{
						result.Add(t);
					}
				}
			}
			return result.ToArray();
		}
	}
}
