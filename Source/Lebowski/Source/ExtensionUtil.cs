namespace Lebowski
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;    
    
    /// <summary>
    /// Provides helper methods to allow an application to be extended
    /// </summary>
    public static class ExtensionUtil
    {
        /// <summary>
        /// Finds all types in loaded assemblies implementing a certain interface
        /// (or inheriting from a class), that have a zero-argument constructor
        /// </summary>
        /// <param name="type">The type that returned types have to be a subtype from</param>
        /// <returns>The types fulfilling the above requirements</returns>
        public static Type[] FindTypesImplementing(Type type)
        {
            List<Type> result = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) 
            {
                foreach (Type t in assembly.GetTypes())
                {
                    if (t.IsClass && t.GetConstructors().Length > 0 && type.IsAssignableFrom(t))
                    {
                        result.Add(t);
                    }
                }
            }
            return result.ToArray();
        }
    }
}
