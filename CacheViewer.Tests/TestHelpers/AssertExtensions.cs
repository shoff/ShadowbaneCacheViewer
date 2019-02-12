namespace CacheViewer.Tests.TestHelpers
{
    using Xunit;

    public static class AssertX
    {
        public static void Equal(int a, uint b)
        {
            Assert.Equal(a, (int) b);
        }

        public static void Equal(uint a, int b)
        {
            Assert.Equal((int)a, b);
        }
    }
}