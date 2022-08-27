﻿namespace Shadowbane.Exporter.Wavefront.Tests;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Cache.IO;

public class ExporterBaseTest
{
    protected readonly RenderableBuilder renderableBuilder;
    protected readonly IFixture fixture;

    protected ExporterBaseTest()
    {
        this.renderableBuilder = new RenderableBuilder();
        this.fixture = new Fixture();
        this.fixture.Customize(new DoNotFillCollectionProperties());
        this.fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => this.fixture.Behaviors.Remove(b));
        this.fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}

public class DoNotFillCollectionProperties : ICustomization
{
    public void Customize(IFixture fixture) => fixture.Customizations.Add(new CollectionPropertyOmitter());
}

public class CollectionPropertyOmitter : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        PropertyInfo? propertyInfo = request as PropertyInfo;
        return propertyInfo != null && propertyInfo.PropertyType.IsGenericType &&
            propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>) ?
                new OmitSpecimen() : new NoSpecimen();
    }
}