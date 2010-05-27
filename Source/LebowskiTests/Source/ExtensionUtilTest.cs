namespace LebowskiTests
{
    using System;
    using Lebowski;
    using NUnit.Framework;
    
    interface ISuperInterface {};
    
    public class A : ISuperInterface
    {
        public A() {}
    };
    public class B : ISuperInterface
    {
        public B() {}
    };
    internal class C : ISuperInterface
    {
        internal C(int foo) {}
    }
    
    interface IUnpopularInterface {};
    internal class D : IUnpopularInterface 
    {
        internal D(string bar) {}
    }
    
    [TestFixture]
    public class ExtensionUtilTest
    {
        /// <summary>
        /// Tests that ExtensionUtil indeed does only return implementing
        /// classes that have a public constructor.
        /// </summary>
        [Test]
        public void FindSuperInterfaceImplementers()
        {
            var types = ExtensionUtil.FindTypesImplementing(typeof(ISuperInterface));
            CollectionAssert.AreEquivalent(new Type[]{ typeof(A), typeof(B) }, types);
        }
        
        /// <summary>
        /// Tests that ExtensionUtil indeed does only return implementing
        /// classes that have a public constructor.
        /// </summary>
        [Test]
        public void FindUnpopularInterfaceImplementers()
        {
            var types = ExtensionUtil.FindTypesImplementing(typeof(IUnpopularInterface));
            CollectionAssert.AreEquivalent(new Type[]{}, types);
        }        
    }
}
