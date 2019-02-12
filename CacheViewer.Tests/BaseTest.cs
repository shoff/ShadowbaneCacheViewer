namespace CacheViewer.Tests
{
    using System.Linq;
    using AutoFixture;

    public abstract class BaseTest
    {
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
    }
}