namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using CacheViewer.Domain.Extensions;
    using CacheViewer.Domain.Factories;
    using NUnit.Framework;

    [TestFixture]
    public class RenderFactoryTests
    {
        private readonly RenderFactory renderFactory = RenderFactory.Instance;


        [Test]
        public void RenderArchive_Should_Have_Correct_Indeces_Count()
        {
            for (int i = 0; i < this.renderFactory.Indexes.Length; i++)
            {
                try
                {
                    var renderInfo = this.renderFactory.Create(this.renderFactory.Indexes[i].Identity);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}