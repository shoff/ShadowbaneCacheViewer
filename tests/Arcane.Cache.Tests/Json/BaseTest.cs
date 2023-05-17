namespace Arcane.Cache.Tests.Json;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

public abstract class BaseTest
{

    protected const string ARCANE_DUMP = "C:\\dev\\ShadowbaneCacheViewer\\ARCANE_DUMP"; // yuck
    protected readonly IFixture fixture;

    protected BaseTest()
    {
        fixture = new Fixture();
        fixture.Customize(new DoNotFillCollectionProperties());
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}

public class DoNotFillCollectionProperties : ICustomization
{
    public void Customize(IFixture fixture) => fixture.Customizations
        .Add(new CollectionPropertyOmitter());
}

public class CollectionPropertyOmitter : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        PropertyInfo propertyInfo = request as PropertyInfo;
        return propertyInfo != null &&
            propertyInfo.PropertyType.IsGenericType &&
            propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>) ?
                new OmitSpecimen() : new NoSpecimen();
    }
}