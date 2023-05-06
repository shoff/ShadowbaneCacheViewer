namespace Shadowbane.Cache.AllTests.IO;

using System.Collections.Generic;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

public class CacheLoaderBaseTest
{
    protected readonly IFixture fixture;

    protected CacheLoaderBaseTest()
    {
        this.fixture = new Fixture();
        this.fixture.Customize(new DoNotFillCollectionProperties());
        this.fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList().ForEach(b => this.fixture.Behaviors.Remove(b));
        this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}

public class DoNotFillCollectionProperties : ICustomization
{
    public void Customize(IFixture fixture) => fixture.Customizations.Add((ISpecimenBuilder)new CollectionPropertyOmitter());
}

public class CollectionPropertyOmitter : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        PropertyInfo propertyInfo = request as PropertyInfo;
        return propertyInfo != null && propertyInfo.PropertyType.IsGenericType 
                                    && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
            ? (object)new OmitSpecimen() : (object)new NoSpecimen();
    }
}