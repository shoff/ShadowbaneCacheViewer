namespace Shadowbane.Exporter.Wavefront
{
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ChaosMonkey.Guards;
    using Models;

    public static class MeshExporter
    {
        private const string SB_MESH_ID = "# MeshId: {0}\r\n";
        private const string USES_CENTIMETERS = "# This file uses centimeters as units for non-parametric coordinates.\r\n";
        private const string DOUBLE_SLASH = "\\";
        private const string DEFAULT_GROUP = "g default\r\n";
        private const string MATERIAL_LIB = "mtllib {0}.mtl\r\n";
        private const string VERTEX = "v {0} {1} {2}\r\n";
        private const string NORMAL = "vn {0} {1} {2}\r\n";
        private const string TEXTURE = "vt {0} {1}\r\n";
        private const string MATERIAL_NAME = "newmtl {0}\r\n";
        private const string USE_MATERIAL = "usemtl {0}\r\n";
        private const string MATERIAL_WHITE = "Ka 1.000 1.000 1.000\r\n";
        private const string MATERIAL_DIFFUSE = "Kd 1.000 1.000 1.000\r\n";
        private const string MATERIAL_SPECULAR = "Ks 0.000 0.000 0.000\r\n ";
        private const string MATERIAL_SPECULAR_NS = "Ns 10.000\r\n";
        private const string MATERIAL_DEFAULT_ILLUMINATION = "illum 4\r\n";
        private const string MAP_TO = "map_Ka {0}\r\nmap_Kd {0}\r\nmap_Ks {0}\r\n";

        public static async Task<bool> ExportAsync(Mesh mesh, string modelDirectory, string modelName = null)
        {
            Guard.IsNotNull(mesh, nameof(mesh));
            Guard.IsNotNullOrWhitespace(modelDirectory, nameof(modelDirectory));
            VerifyModelDirectory(modelDirectory);
            var mainStringBuilder = new StringBuilder();
            var materialBuilder = new StringBuilder();
            // todo - not all objects seem to have names
            modelName ??= string.Join(string.Empty, "Mesh_", mesh.Identity.ToString(CultureInfo.InvariantCulture));
            
            mainStringBuilder.AppendLine(MayaObjHeaderFactory.Instance.Create(modelName));
            mainStringBuilder.AppendFormat(SB_MESH_ID, mesh.Identity);
            mainStringBuilder.AppendFormat(MATERIAL_LIB, modelName);
            mainStringBuilder.Append($"{USES_CENTIMETERS}\r\n");
            mainStringBuilder.Append(DEFAULT_GROUP);
            var meshName = string.Join(string.Empty, "Mesh_", mesh.Identity.ToString(CultureInfo.InvariantCulture));
            var indexMaterialDictionary = new Dictionary<string, string>
            {
                {meshName, string.Empty}
            };
            // do the textures first because this is the most likely place to have an exception thrown. Die young leave a beautiful corpse.
            SaveTextures(mesh, meshName, modelDirectory, indexMaterialDictionary);
            // now create the material entry for the mesh
            // TODO handle extra maps?
            AppendMaterial(indexMaterialDictionary[meshName], $"Mesh_{mesh.Identity}", materialBuilder);
            // v 
            mainStringBuilder.Append(BuildVerts(mesh));
            // vt
            mainStringBuilder.Append(BuildTextures(mesh));
            // vn
            mainStringBuilder.Append(BuildNormals(mesh));
            // now build the faces
            BuildFaces(mesh, mainStringBuilder);

            using var fs = new FileStream($"{modelDirectory}{DOUBLE_SLASH}{modelName}.obj", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var writer = new StreamWriter(fs);
            await writer.WriteAsync(mainStringBuilder.ToString());

            // save the material
            var mtlFile = $"{modelDirectory}{DOUBLE_SLASH}{modelName}.mtl";
            if (File.Exists(mtlFile))
            {
                File.Delete(mtlFile);
            }

            using var fs1 = new FileStream($"{modelDirectory}{DOUBLE_SLASH}{modelName}.mtl", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            using var writer1 = new StreamWriter(fs1);
            await writer1.WriteAsync(materialBuilder.ToString());
            return true;
        }

        private static void VerifyModelDirectory(string modelDirectory)
        {
            if (!Directory.Exists(modelDirectory))
            {
                Directory.CreateDirectory(modelDirectory);
            }
        }

        private static void AppendMaterial(string mapName, string materialName, StringBuilder sb)
        {
            sb.AppendFormat(MATERIAL_NAME, materialName);
            sb.Append(MATERIAL_WHITE);
            sb.Append(MATERIAL_DIFFUSE);
            sb.Append(MATERIAL_SPECULAR);
            sb.Append(MATERIAL_SPECULAR_NS);
            sb.Append(MATERIAL_DEFAULT_ILLUMINATION);
            sb.AppendFormat(MAP_TO, mapName.Substring(mapName.LastIndexOf('\\') + 1));
        }

        private static string BuildVerts(Mesh mesh)
        {
            var sb = new StringBuilder();
            foreach (var v in mesh.Vertices)
            {
                sb.AppendFormat(VERTEX, v[0].ToString("0.000000"), v[1].ToString("0.000000"), v[2].ToString("0.000000"));
            }

            return sb.ToString();
        }

        private static string BuildTextures(Mesh mesh)
        {
            var sb = new StringBuilder();
            foreach (var t in mesh.TextureVectors)
            {
                sb.AppendFormat(TEXTURE, t[0].ToString("0.000000"), t[1].ToString("0.000000"));
            }

            return sb.ToString();
        }

        private static string BuildNormals(Mesh mesh)
        {
            var sb = new StringBuilder();
            foreach (var vn in mesh.Normals)
            {
                sb.AppendFormat(NORMAL, vn[0].ToString("0.000000"), vn[1].ToString("0.000000"), vn[2].ToString("0.000000"));
            }

            return sb.ToString();
        }

        //private void BuildFaces(Mesh mesh, StringBuilder sb)
        //{
        //    sb.AppendLine("s off"); // smoothing groups off
        //    sb.AppendLine($"g Mesh_{mesh.Id}");
        //    sb.AppendLine($"usemtl Mesh_{mesh.Id}");

        //    for (int i = 0; i < mesh.Indices.Count; i += 3)
        //    {
        //        var a = (ushort)(mesh.Indices[i].Position);
        //        var b = (ushort)(mesh.Indices[i].TextureCoordinate);
        //        var c = (ushort)(mesh.Indices[i].Normal);

        //        var d = (ushort)(mesh.Indices[i + 1].Position);
        //        var e = (ushort)(mesh.Indices[i + 1].TextureCoordinate);
        //        var f = (ushort)(mesh.Indices[i + 1].Normal);

        //        var g = (ushort)(mesh.Indices[i + 2].Position);
        //        var h = (ushort)(mesh.Indices[i + 2].TextureCoordinate);
        //        var j = (ushort)(mesh.Indices[i + 3].Normal);

        //        sb.Append("f " + a + @"/" + b + @"/" + c + " " + d + @"/" + e + @"/" + f + " " + g + @"/" + h + @"/" + j + "\r\n");
        //    }
        //}

        private static void BuildFaces(Mesh mesh, StringBuilder sb)
        {
            int currentIndexCount = 0;

            currentIndexCount++;

            // TODO this does not spit out the faces the same as the exporter from Maya does
            sb.AppendLine("s off"); // smoothing groups off
            sb.AppendLine($"g Mesh_{mesh.Identity}");
            sb.AppendLine($"usemtl Mesh_{mesh.Identity}");

            foreach (var wavefrontVertex in mesh.Indices)
            {
                var a = (ushort)(wavefrontVertex.Position + currentIndexCount);
                var b = (ushort)(wavefrontVertex.TextureCoordinate + currentIndexCount);
                var c = (ushort)(wavefrontVertex.Normal + currentIndexCount);

                sb.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c + @"/" + c + @"/" + c + "\r\n");
            }

        }

        private static void SaveTextures(Mesh mesh, string meshName, 
            string modelDirectory,
            Dictionary<string, string> indexMaterialDictionary)
        {
            if (!mesh.Textures.Any())
            {
                return;
            }

            for (var i = 0; i < mesh.Textures.Count; i++)
            {
                var texture = mesh.Textures[i];
                if (texture?.Image == null)
                {
                    // OOOPS.
                    continue;
                }

                var textureFile = $"{modelDirectory}{DOUBLE_SLASH}{mesh.Identity.ToString(CultureInfo.InvariantCulture).Replace(" ", "_")}_{i}.png";
                texture.Image?.Save(textureFile, ImageFormat.Png);
                var usmtl = string.Format(USE_MATERIAL, $"Mesh_{mesh.Identity}");

                if (indexMaterialDictionary[meshName] != string.Empty)
                {
                    // we're on more than one texture here ... shit
                    indexMaterialDictionary.Add($"{meshName}_{i}", textureFile);
                }
                else
                {
                    indexMaterialDictionary[meshName] = textureFile;
                }

                // this should go before each set of faces.
                // sb.AppendFormat(UseMaterial, $"Mesh_{mesh.Id}");
            }
        }

        private static void CreateObject(Mesh mesh, StringBuilder mainStringBuilder, StringBuilder materialBuilder, string directory, string meshName = null)
        {
            var mapFiles = new List<string>();

            if (mesh.Textures.Any())
            {

                for (var i = 0; i < mesh.Textures.Count(); i++)
                {
                    var texture = mesh.Textures[i];

                    var mapName = directory + "\\" + mesh.Identity.ToString(CultureInfo.InvariantCulture).Replace(" ", "_") + "_" + i + ".png";
                    mapFiles.Add(mapName);
                    texture.Image.Save(mapName, ImageFormat.Png);

                    mainStringBuilder.AppendFormat(USE_MATERIAL, $"Mesh_{mesh.Identity}");
                    mainStringBuilder.AppendLine("s off");
                }
            }


            if (mapFiles.Count > 0)
            {
                CreateMaterial(mapFiles[0], materialBuilder, meshName, null);
            }

            foreach (var v in mesh.Vertices)
            {
                mainStringBuilder.AppendFormat(VERTEX, v[0].ToString("0.0#####"), v[1].ToString("0.0#####"),
                    v[2].ToString("0.0#####"));
            }

            foreach (var t in mesh.TextureVectors)
            {
                mainStringBuilder.AppendFormat(TEXTURE, t[0].ToString("0.000000"), t[1].ToString("0.000000"));
            }

            foreach (var vn in mesh.Normals)
            {
                mainStringBuilder.AppendFormat(NORMAL, vn[0].ToString("0.000000"), vn[1].ToString("0.000000"),
                    vn[2].ToString("0.000000"));
            }

            // TODO this does not spit out the faces the same as the exporter from Maya does
            // g Mesh_124163:default1
            // usemtl initialShadingGroup
            mainStringBuilder.AppendLine($"g Mesh_{mesh.Identity}:Mesh_{mesh.Identity}");
            mainStringBuilder.AppendLine($"usemtl Mesh_{mesh.Identity}");

            foreach (var wavefrontVertex in mesh.Indices)
            {
                var a = (ushort)(wavefrontVertex.Position + 1);
                var b = (ushort)(wavefrontVertex.TextureCoordinate + 1);
                var c = (ushort)(wavefrontVertex.Normal + 1);

                mainStringBuilder.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c +
                    @"/" + c +
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

        private static void CreateMaterial(string mapName, StringBuilder materialBuilder, string materialName = null, string name = null)
        {
            materialBuilder.AppendFormat(MATERIAL_NAME, materialName ?? name);
            materialBuilder.Append(MATERIAL_WHITE);
            materialBuilder.Append(MATERIAL_DIFFUSE);
            materialBuilder.Append(MATERIAL_SPECULAR);
            materialBuilder.Append(MATERIAL_SPECULAR_NS);
            materialBuilder.Append(MATERIAL_DEFAULT_ILLUMINATION);
            materialBuilder.AppendFormat(MAP_TO, mapName.Substring(mapName.LastIndexOf('\\') + 1));
        }
    }
}