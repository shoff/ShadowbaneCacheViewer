
namespace CacheViewer.Domain.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CacheViewer.Domain.Archive;
    using CacheViewer.Domain.Factories;
    using CacheViewer.Domain.Models;
    using CacheViewer.Domain.Parsers;
    using CacheViewer.Domain.Utility;
    using NLog;

    /// <summary>
    /// </summary>
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    public class MeshOnlyObjExporter
    {

        private const string SbRenderId = "# RenderId: {0}\r\n";
        private const string UsesCentimeters = "# This file uses centimeters as units for non-parametric coordinates.\r\n";
        private const string DefaultGroup = "g default\r\n";
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
        private readonly string modelDirectory = FileLocations.Instance.GetExportFolder();
        private string name;
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private void ObjectVariant()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjExporter" /> class.
        /// </summary>
        /// The directory specified by <paramref>
        ///   <name>path</name>
        /// </paramref>
        /// is a file.-or-The network name is not known.
        /// The caller does not have the required permission.
        /// The specified path is invalid (for example, it is on an unmapped drive).
        /// <cref>DirectoryNotFoundException</cref>
        public MeshOnlyObjExporter()
        {
            if (!Directory.Exists(this.modelDirectory))
            {
                // ReSharper disable once ExceptionNotDocumented
                Directory.CreateDirectory(this.modelDirectory);
            }
        }

        /// <summary>
        /// Exports the specified Mesh.
        /// </summary>
        /// <param name="mesh">
        /// The Mesh.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// </returns>
        public async Task<bool> ExportAsync(Mesh mesh, string name = null)
        {

            try
            {
                StringBuilder mainStringBuilder = new StringBuilder();
                StringBuilder materialBuilder = new StringBuilder();
                mainStringBuilder.Append(UsesCentimeters);

                // todo - not all objects seem to have names
                this.name = name ?? string.Join(string.Empty, "Mesh_", mesh.CacheIndex.Identity.ToString(CultureInfo.InvariantCulture));

                mainStringBuilder.Append(MayaObjHeaderFactory.Instance.Create(mesh.CacheIndex.Identity));
                mainStringBuilder.AppendFormat(SbRenderId, mesh.CacheIndex.Identity);
                mainStringBuilder.AppendFormat(MaterialLib, this.name);
                mainStringBuilder.Append(DefaultGroup);

                // save the obj
                this.CreateObject(mesh, mainStringBuilder, materialBuilder, this.modelDirectory);

                using (
                    var fs = new FileStream(
                        this.modelDirectory + "\\" + this.name + ".obj", 
                        FileMode.Create, 
                        FileAccess.ReadWrite, 
                        FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        await writer.WriteAsync(mainStringBuilder.ToString());
                    }
                }

                // save the material
                string mtlFile = this.modelDirectory + "\\" + this.name + ".mtl";
                if (File.Exists(mtlFile))
                {
                    File.Delete(mtlFile);
                }

                using (
                    var fs1 = new FileStream(
                        this.modelDirectory + "\\" + this.name + ".mtl", 
                        FileMode.Create, 
                        FileAccess.ReadWrite, 
                        FileShare.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fs1))
                    {
                        await writer.WriteAsync(materialBuilder.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="mesh">
        /// </param>
        /// <param name="mainStringBuilder">
        /// </param>
        /// <param name="materialBuilder">
        /// </param>
        /// <param name="directory">
        /// </param>
        private void CreateObject(Mesh mesh, StringBuilder mainStringBuilder, StringBuilder materialBuilder, string directory)
        {

            List<string> mapFiles = new List<string>();

            if (mesh.Textures.Any())
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


            if (mapFiles.Count > 0)
            {
                this.CreateMaterial(mapFiles[0], materialBuilder);
            }

            foreach (var v in mesh.Vertices)
            {
                mainStringBuilder.AppendFormat(Vertice, v[0].ToString("0.0#####"), v[1].ToString("0.0#####"), v[2].ToString("0.0#####"));
            }

            foreach (var vn in mesh.Normals)
            {
                mainStringBuilder.AppendFormat(Normal, vn[0].ToString("0.0#####"), vn[1].ToString("0.0#####"), vn[2].ToString("0.0#####"));
            }

            foreach (var t in mesh.TextureVectors)
            {
                mainStringBuilder.AppendFormat(Texture, t[0].ToString("0.0#####"), t[1].ToString("0.0#####"));
            }

            // TODO this does not spit out the faces the same as the exporter from Maya does
            foreach (WavefrontVertex wavefrontVertex in mesh.Indices)
            {
                ushort a = (ushort)(wavefrontVertex.Position + 1);
                ushort b = (ushort)(wavefrontVertex.TextureCoordinate + 1);
                ushort c = (ushort)(wavefrontVertex.Normal + 1);

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
        // # be the same as the ambient texture map)
        // map_Ks lenna.tga           # the specular texture map
        // map_d lenna_alpha.tga      # the alpha texture map
        // map_bump lenna_bump.tga    # the bump map
        // bump lenna_bump.tga        # some implementations use 'bump' instead of 'map_Bump'
        /// <summary>
        /// </summary>
        /// <param name="mapName">
        /// </param>
        /// <param name="materialBuilder">
        /// </param>
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

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static MeshOnlyObjExporter Instance
        {
            get { return new MeshOnlyObjExporter(); }
        }
    }
}