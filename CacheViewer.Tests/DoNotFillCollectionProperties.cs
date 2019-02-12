namespace CacheViewer.Tests
{
    using AutoFixture;

    public class DoNotFillCollectionProperties : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new CollectionPropertyOmitter());
        }
    }
}