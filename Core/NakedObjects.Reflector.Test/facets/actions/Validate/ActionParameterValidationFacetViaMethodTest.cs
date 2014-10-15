// Copyright � Naked Objects Group Ltd ( http://www.nakedobjects.net). 
// All Rights Reserved. This code released under the terms of the 
// Microsoft Public License (MS-PL) ( http://opensource.org/licenses/ms-pl.html) 

using System.Reflection;
using Moq;
using NakedObjects.Architecture.Adapter;
using NakedObjects.Architecture.Facets;
using NUnit.Framework;

namespace NakedObjects.Reflector.DotNet.Facets.Actions.Validate {
    [TestFixture]
    public class ActionParameterValidationFacetViaMethodTest {
        #region Setup/Teardown

        [SetUp]
        public void SetUp() {
            IFacetHolder holder = null;
            var customer = new Customer17();

            var mock = new Mock<INakedObject>();
            target = mock.Object;

            mock.Setup(no => no.Object).Returns(customer);

            MethodInfo method = typeof (Customer17).GetMethod("Validate0SomeAction");
            facet = new ActionParameterValidationFacetViaMethod(method, 0, holder);
        }

        [TearDown]
        public void TearDown() {}

        #endregion

        private INakedObject target;
        private ActionParameterValidationFacetViaMethod facet;

        [Test]
        public void Test1() {
            var mock = new Mock<INakedObject>();
            INakedObject value = mock.Object;
            mock.Setup(no => no.Object).Returns(10);
            Assert.That(facet.InvalidReason(target, value), Is.Null);
        }


        [Test]
        public void Test2() {
            var mock = new Mock<INakedObject>();
            INakedObject value = mock.Object;
            mock.Setup(no => no.Object).Returns(-7);
            Assert.That(facet.InvalidReason(target, value), Is.EqualTo("must be positive"));
        }
    }


    internal class Customer17 {
        public void SomeAction(int x, long y, long z) {}

        public string Validate0SomeAction(int x) {
            return x > 0 ? null : "must be positive";
        }

        public string Validate1SomeAction(long x) {
            return null;
        }
    }
}