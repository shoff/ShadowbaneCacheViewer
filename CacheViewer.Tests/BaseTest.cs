namespace CacheViewer.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using AutoFixture;

    public abstract class BaseTest
    {
        private static readonly Random random = new Random((int) DateTime.Now.Ticks);

        /// <summary>
        ///     AutoFixture is an open source library for .NET designed to minimize the 'Arrange'
        ///     phase of your unit tests in order to maximize maintainability. Its primary goal is to
        ///     allow developers to focus on what is being tested rather than how to setup the test
        ///     scenario, by making it easier to create object graphs containing test data.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected IFixture fixture;

        protected BaseTest()
        {
            this.fixture = new Fixture().Customize(new DoNotFillCollectionProperties());
            this.fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => this.fixture.Behaviors.Remove(b));
            this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        private static string RandomValue(int length)
        {
            StringBuilder sb = new StringBuilder();
            while (sb.Length < length)
            {
                int randomValue = random.Next(65, 90); // upper case letters
                sb.Append((Char)randomValue);
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// Gets the char.
        /// </summary>
        /// <returns></returns>
        public static char GetChar()
        {
            return GetString(1).ToCharArray()[0];
        }
        
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns></returns>
        public static string GetString(int length = 10)
        {
            return RandomValue(length);
        }

        public static class RandomString
        {
            /// <summary>
            ///     Builds a random string of the specified size.
            /// </summary>
            /// <param name="size">The size.</param>
            /// <returns></returns>
            public static string Build(int size)
            {
                var builder = new StringBuilder();
                for (var i = 0; i < size; i++)
                {
                    var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }

                return builder.ToString();
            }
        }
    }
}