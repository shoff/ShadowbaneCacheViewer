﻿namespace CacheViewer.Domain.Exporters
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IPrefabObjExporter
    {
        Task CreateIndividualObjFiles(List<Mesh> meshModels, string modelName);
        Task CreateSingleObjFile(List<Mesh> meshModels, string modelName);
        string ModelDirectory { get; set; }
    }
}