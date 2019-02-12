namespace CacheViewer.Tests.Domain.Extensions
{
    using System;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Geometry;
    using Xunit;

    public class Vector3ExtensionsTests
    {
        private readonly Vector3 vector3 = new Vector3(1.33823f, 2.3356f, -4.008f);

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ToVector3_Should_Throw_If_VectorString_Is_Null_Or_Empty(string vectorString)
        {
            Assert.Throws<ArgumentException>(() => vectorString.ToVector3());
        }

        [Fact]
        public void ToString_Should_Create_Correct_Representation_For_Persisting()
        {
            var actual = this.vector3.ToString();
            Console.WriteLine(actual);
        }

        [Fact]
        public void ToVector3_Creates_Correct_Vector()
        {
            var vecString = this.vector3.ToString();
            var newVector = vecString.ToVector3();
            Assert.Equal(this.vector3, newVector);
        }
    }
}