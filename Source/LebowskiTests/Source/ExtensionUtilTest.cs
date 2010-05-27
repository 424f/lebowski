namespace LebowskiTests
{
    using System;
    using Lebowski;
    using NUnit.Framework;
    
    interface ISuperInterface {};
    
    class A : ISuperInterface
    {
        A() {}
    };
    class B : ISuperInterface
    {
        B() {}
    };
    class C : ISuperInterface
    {
        C(int foo) {}
    }
    
    interface IUnpopularInterface {};
    class D : IUnpopularInterface 
    {
        D(string bar) {}
    }
    
    [TestFixture]
    public class ExtensionUtilTest
    {
        /// <summary>
        /// Tests that ExtensionUtil indeed does only return implementing
        /// classes that have a 0-argument constructor.
        /// </summary>
        [Test]
        public void FindSuperInterfaceImplementers()
        {
            var types = ExtensionUtil.FindTypesImplementing(typeof(ISuperInterface));
            CollectionAssert.AreEquivalent(new Type[]{ typeof(A), typeof(B) }, types);
        }
        
        /// <summary>
        /// Tests that ExtensionUtil indeed does only return implementing
        /// classes that have a 0-argument constructor.
        /// </summary>
        [Test]
        public void FindUnpopularInterfaceImplementers()
        {
            var types = ExtensionUtil.FindTypesImplementing(typeof(ISuperInterface));
            CollectionAssert.AreEquivalent(new Type[]{}, types);
        }        
    }
}
