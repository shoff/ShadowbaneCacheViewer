using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CacheViewer.Domain.Archive;
using CacheViewer.Domain.Factories;
using CacheViewer.Domain.Models;
using CacheViewer.Domain.Models.Exportable;
using CacheViewer.Domain.Utility;

namespace CacheViewer.Domain.Exporters
{
    public class ObjExporter
    {
        private const string SbRenderId = "# RenderId: {0}\r\n";
        private const string MaterialLib = "mtllib {0}.mtl\r\n";
        private const string Vertice = "v {0} {1} {2}\r\n";
        private const string Normal = "vn {0} {1} {2}\r\n";
        private const string Texture = "vt {0} {1}\r\n";
        private const string MaterialName = "newmtl {0}\r\n";
        private const string MaterialWhite = "Ka 1.000 1.000 1.000\r\n";
        private const string MaterialDiffuse = "Kd 1.000 1.000 1.000\r\n";
        private const string MaterialSpecular = "Ks 0.000 0.000 0.000\r\n ";
        private const string MaterialSpecualrNs = "Ns 10.000";
        private const string MaterialDefaultIllumination = "illum 2\r\n";
        private const string MapTo = "map_Ka {0}\r\nmap_Kd {0}\r\nmap_Ks {0}\r\n";
        private readonly string modelDirectory; // = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\Models\\");
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjExporter"/> class.
        /// </summary>
        public ObjExporter()
        {
            this.modelDirectory = FileLocations.Instance.GetExportFolder();
        }

        public void Export(ICacheObject cacheObject)
        {
            StringBuilder mainStringBuilder = new StringBuilder();
            StringBuilder materialBuilder = new StringBuilder();
            
            // todo - not all objects seem to have names
            this.name = string.IsNullOrEmpty(cacheObject.Name) 
                ? cacheObject.CacheIndex.Identity + "_" 
                : cacheObject.Name.Replace(" ", "_");

            string exportDirectory = this.EnsureDirectory(this.name);
            mainStringBuilder.Append(MayaObjHeaderFactory.Instance.Create(cacheObject.CacheIndex.Identity));
            mainStringBuilder.AppendFormat(MaterialLib, this.name);

            // we'll treat each renderInfo as a separate Mesh for now.
            // foreach (var obj in cacheObject.RenderInfoList)
            
            // cacheObject.RenderList.AsParallel().ForAll(obj => this.CreateObject(obj, cacheObject, modelDirectory));
            // save the obj

            using (var fs = new FileStream(exportDirectory + "\\" + this.name + ".obj", FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(mainStringBuilder.ToString());
                }
            }

            // save the material
            string mtlFile = exportDirectory + "\\" + this.name + ".mtl";
            if (File.Exists(mtlFile))
            {
                File.Delete(mtlFile);
            }

            using (
                var fs1 = new FileStream(exportDirectory + "\\" + this.name + ".mtl", FileMode.Create, FileAccess.ReadWrite,
                    FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fs1))
                {
                    writer.Write(materialBuilder.ToString());
                }
            }
        }

        public void CreateObject(Mesh mesh, StringBuilder mainStringBuilder, 
            StringBuilder materialBuilder, string directory)
        {
            mainStringBuilder.AppendFormat(SbRenderId, mesh.CacheIndex.Identity);

            List<string> mapFiles = new List<string>();


            if ((mesh.Textures != null) && (mesh.Textures.Any()))
            {
                Textures archive = (Textures)ArchiveFactory.Instance.Build(CacheFile.Textures);
                for (int i = 0; i < mesh.Textures.Count(); i++)
                {
                    var texture = mesh.Textures[i];
                    var asset = archive[texture.TextureId];
                    using (var map = mesh.Textures[i].TextureMap(asset.Item1))
                    {
                        var mapName = directory + "\\" +
                            mesh.Id.ToString(CultureInfo.InvariantCulture).Replace(" ", "_") + "_" + i + ".jpg";
                        mapFiles.Add(mapName);
                        map.Save(mapName, ImageFormat.Jpeg);
                    }
                }
            }

            // for now let's just see if we can even get one to work
            if (mapFiles.Count > 0)
            {
                this.CreateMaterial(mapFiles[0], materialBuilder);
            }

            foreach (var v in mesh.Vertices)
            {
                mainStringBuilder.AppendFormat(Vertice, v[0], v[1], v[2]);
            }

            foreach (var vn in mesh.Normals)
            {
                mainStringBuilder.AppendFormat(Normal, vn[0], vn[1], vn[2]);
            }

            foreach (var t in mesh.TextureVectors)
            {
                mainStringBuilder.AppendFormat(Texture, t[0], t[1]);
            }

            for (int i = 0; i < mesh.Indices.Count; i++)
            {
                ushort a = (ushort)(mesh.Indices[i].Position + 1);
                ushort b = (ushort)(mesh.Indices[i].TextureCoordinate + 1);
                ushort c = (ushort)(mesh.Indices[i].Normal + 1);

                mainStringBuilder.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c + @"/" + c +
                    @"/" + c + "\r\n");
            }
        }

        // Multiple illumination models are available, per material. These are enumerated as follows:
        // 0. Color on and Ambient off
        // 1. Color on and Ambient on
        // 2. Highlight on
        // 3. Reflection on and Ray trace on
        // 4. Transparency: Glass on, Reflection: Ray trace on
        // 5. Reflection: Fresnel on and Ray trace on
        // 6. Transparency: Refraction on, Reflection: Fresnel off and Ray trace on
        // 7. Transparency: Refraction on, Reflection: Fresnel on and Ray trace on
        // 8. Reflection on and Ray trace off
        // 9. Transparency: Glass on, Reflection: Ray trace off
        // 10. Casts shadows onto invisible surfaces

        // example
        // newmtl Textured
        // Ka 1.000 1.000 1.000
        // Kd 1.000 1.000 1.000
        // Ks 0.000 0.000 0.000
        // d 1.0
        // illum 2
        // map_Ka lenna.tga           # the ambient texture map
        // map_Kd lenna.tga           # the diffuse texture map (most of the time, it will
        //                            # be the same as the ambient texture map)
        // map_Ks lenna.tga           # the specular texture map
        // map_d lenna_alpha.tga      # the alpha texture map
        // map_bump lenna_bump.tga    # the bump map
        // bump lenna_bump.tga        # some implementations use 'bump' instead of 'map_Bump'
        private void CreateMaterial(string mapName, StringBuilder materialBuilder)
        {
            materialBuilder.AppendFormat(MaterialName, this.name);
            materialBuilder.Append(MaterialWhite);
            materialBuilder.Append(MaterialDiffuse);
            materialBuilder.Append(MaterialSpecular);
            materialBuilder.Append(MaterialSpecualrNs);
            materialBuilder.Append(MaterialDefaultIllumination);
            materialBuilder.AppendFormat(MapTo, mapName.Substring(mapName.LastIndexOf('\\') + 1));
        }

        private string EnsureDirectory(string name)
        {
            string fullName = Path.Combine(this.modelDirectory, name);

            if (Directory.Exists(fullName))
            {
                Directory.Delete(fullName, true);
            }
            Directory.CreateDirectory(fullName);
            return fullName;
        }
    }
}