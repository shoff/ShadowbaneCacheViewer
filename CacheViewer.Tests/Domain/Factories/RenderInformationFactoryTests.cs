namespace CacheViewer.Tests.Domain.Factories
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Factories;
    using Newtonsoft.Json;
    using NUnit.Framework;

    [TestFixture]
    public class RenderInformationFactoryTests
    {
        private readonly RenderInformationFactory renderInformationFactory = RenderInformationFactory.Instance;

        [Test, Explicit]
        public async Task Temp_OutPut_All_To_Files()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "RenderIndexes";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
            {
                await this.renderInformationFactory.RenderArchive.SaveToFile(index, folder);
            }
        }

        [Test, Explicit]
        public void Temp_OutPut_All_To_Json()
        {
            var folder = AppDomain.CurrentDomain.BaseDirectory + "RenderIndexes";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (var index in this.renderInformationFactory.RenderArchive.CacheIndices)
            {
                // await this.renderInformationFactory.RenderArchive.SaveToFile(index, folder);
                var render = this.renderInformationFactory.Create(index.Identity, index.Order, true);
                var renderJson = JsonConvert.SerializeObject(render);
                File.WriteAllText($"{folder}\\{this.renderInformationFactory.RenderArchive.Name}_{index.Identity}.json",
                    renderJson);
            }
        }

        [Test]
        public void RenderArchive_Should_Have_Correct_Indeces_Count()
        {
            //for (int i = 0; i < this.renderInformationFactory.Indexes.Length; i++)
            //{
            //    try
            //    {
            //        var renderInfo = this.renderInformationFactory.CreateAndParse(this.renderInformationFactory.Indexes[i].Identity, addByteData: true);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
        }
    }
}