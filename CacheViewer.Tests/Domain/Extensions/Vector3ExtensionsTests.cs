using System;
using CacheViewer.Domain.Extensions;
using NUnit.Framework;

namespace CacheViewer.Tests.Domain.Extensions
{
    using OpenTK;

    [TestFixture]
    public class Vector3ExtensionsTests
    {
        private readonly Vector3 vector3 = new Vector3(1.33823f, 2.3356f, -4.008f);


        [Test]
        public void ToString_Should_Create_Correct_Representation_For_Persisting()
        {
            var actual = this.vector3.ToString();
            Console.WriteLine(actual);
        }

        [TestCase(null), Ignore("")]
        [TestCase("")]
        public void ToVector3_Should_Throw_If_VectorString_Is_Null_Or_Empty(string vectorString)
        {
            Assert.Throws<ArgumentNullException>(() => vectorString.ToVector3());
        }

        [Test]
        public void ToVector3_Creates_Correct_Vector()
        {
            string vecString = this.vector3.ToString();
            Vector3 newVector = vecString.ToVector3();
            Assert.AreEqual(this.vector3, newVector);
        }
    }
}