namespace CacheViewer.Tests
{
    using AutoFixture;

    public class DoNotFill<T> : ICustomization
        where T : class
    {
        /// <summary>
        ///     Customizes the specified fixture.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreSpecimen<T>());
        }
    }
}