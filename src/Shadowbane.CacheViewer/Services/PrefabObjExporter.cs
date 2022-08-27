namespace Shadowbane.CacheViewer.Services;

using System.Globalization;
using System.Text;
using Cache;
using Cache.CacheTypes;
using Cache.IO;
using ChaosMonkey.Guards;
using Exporter.Wavefront;
using Serilog;
using SixLabors.ImageSharp;

public class PrefabObjExporter : IPrefabObjExporter
{

    private const string USES_CENTIMETERS = "# This file uses centimeters as units for non-parametric coordinates.\r\n\r\n";
    private const string MATERIAL_LIB = "mtllib {0}.mtl\r\n";
    private const string VERTICE = "v {0} {1} {2}\r\n";
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

    private readonly MeshFactory meshFactory;
    private readonly TextureCache texturesArchive = ArchiveLoader.TextureArchive;
    private readonly Dictionary<string, string> indexMaterialDictionary = new();
    private readonly StringBuilder prefab = new();
    private readonly StringBuilder material = new();

    public PrefabObjExporter()
    {
        this.meshFactory = new MeshFactory();
        if (!string.IsNullOrEmpty(this.ModelDirectory))
        {
            if (!Directory.Exists(this.ModelDirectory))
            {
                // ReSharper disable once ExceptionNotDocumented
                Directory.CreateDirectory(this.ModelDirectory);
            }
        }
    }

    public static MeshOnlyObjExporter Instance => new MeshOnlyObjExporter();

    public async Task CreateIndividualObjFiles(ICollection<IMesh>? meshModels, string modelName)
    {
        Guard.IsNotNull(meshModels, nameof(meshModels));
        Guard.IsNotEmpty(meshModels, nameof(meshModels));
        Guard.IsNotNullOrWhitespace(modelName, nameof(modelName));

        foreach (var mesh in meshModels!)
        {
            var cacheIndex = ArchiveLoader.MeshArchive.CacheIndices.FirstOrDefault(c => c.identity == mesh.Identity);
            var createdMesh = this.meshFactory.Create(cacheIndex);

            if (createdMesh == null)
            {
                Log.Error($"could not create a mesh for mesh as cache index {cacheIndex.identity}");
                continue;
            }

            // this is clearly not right, meshfactory does not create the textures
            foreach (var rt in mesh.Textures)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (rt == null)
                {
                    Log.Error("Null mesh in {Mesh} Texture Collection", mesh.Identity);
                    continue;
                }
                // TextureFactory.Instance.Build(rt.TextureId);
                createdMesh.Textures.Add(rt);
            }

            await Instance.ExportAsync(createdMesh, $"{modelName}_{createdMesh.Identity}", this.ModelDirectory);
        }
    }

    public async Task CreateSingleObjFile(ICollection<IMesh>? meshModels, string? modelName)
    {
        Guard.IsNotNull(meshModels, nameof(meshModels));
        Guard.IsNotEmpty(meshModels, nameof(meshModels));
        Guard.IsNotNullOrWhitespace(modelName, nameof(modelName));

        // Builds the .obj header/information header
        this.prefab.AppendLine(MayaObjHeaderFactory.Instance.Create(modelName));
        this.prefab.Append(USES_CENTIMETERS); // TODO validate this flag
        this.prefab.AppendFormat(MATERIAL_LIB, modelName);
        ushort currentCount = 0;
        foreach (var mesh in meshModels!)
        {
            var vertexBuilder = new StringBuilder();
            var normalsBuilder = new StringBuilder();
            var texturesBuilder = new StringBuilder();

            try
            {
                var meshName = string.Join(string.Empty, "Mesh_", mesh.Identity.ToString(CultureInfo.InvariantCulture));
                this.indexMaterialDictionary.Add(meshName, string.Empty);

                // do the textures first because this is the most likely place to have an exception thrown.
                this.SaveTextures(mesh, meshName);

                // now create the material entry for the mesh
                // TODO handle extra maps?
                this.AppendMaterial(this.indexMaterialDictionary[meshName], $"Mesh_{mesh.Identity}");

                // v 
                vertexBuilder.AppendLine("g default");
                vertexBuilder.Append(this.BuildVerts(mesh));

                // vt
                texturesBuilder.Append(this.BuildTextures(mesh));

                // vn
                normalsBuilder.Append(this.BuildNormals(mesh));

                string faces = this.BuildFaces(mesh, currentCount);
                currentCount += (ushort)mesh.VertexCount;

                // combine them all
                this.prefab.Append(vertexBuilder.ToString());
                this.prefab.Append(texturesBuilder.ToString());
                this.prefab.Append(normalsBuilder);
                this.prefab.Append(faces);
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }
        }

        using (var fs = new FileStream(this.ModelDirectory + "\\" + modelName + ".obj",
                FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            using (var writer = new StreamWriter(fs))
            {
                await writer.WriteAsync(this.prefab.ToString());
            }
        }

        // save the material
        var mtlFile = this.ModelDirectory + "\\" + modelName + ".mtl";
        if (File.Exists(mtlFile))
        {
            File.Delete(mtlFile);
        }

        using (var fs1 = new FileStream(this.ModelDirectory + "\\" + modelName + ".mtl",
            FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
        {
            using (var writer = new StreamWriter(fs1))
            {
                await writer.WriteAsync(this.material.ToString());
            }
        }
    }

    public string? ModelDirectory { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}\\Models";

    private string BuildVerts(IMesh mesh)
    {
        var sb = new StringBuilder();
        foreach (var v in mesh.Vertices)
        {
            sb.AppendFormat(VERTICE, v.X.ToString("0.0#####"), v.Y.ToString("0.0#####"), v.Z.ToString("0.0#####"));
        }

        return sb.ToString();
    }

    private string BuildTextures(IMesh mesh)
    {
        var sb = new StringBuilder();
        foreach (var t in mesh.TextureVectors)
        {
            sb.AppendFormat(TEXTURE, t.X.ToString("0.000000"), t.Y.ToString("0.000000"));
        }

        return sb.ToString();
    }

    private string BuildNormals(IMesh mesh)
    {
        var sb = new StringBuilder();
        foreach (var vn in mesh.Normals)
        {
            sb.AppendFormat(NORMAL, vn.X.ToString("0.000000"), vn.Y.ToString("0.000000"), vn.Z.ToString("0.000000"));
        }

        return sb.ToString();
    }

    private string BuildFaces(IMesh mesh, ushort? vertexCount)
    {
        vertexCount ??= 0;
        var sb = new StringBuilder();
        sb.AppendLine("s off"); // smoothing groups off
        sb.AppendLine($"g Mesh_{mesh.Identity}");
        sb.AppendLine($"usemtl Mesh_{mesh.Identity}");

        foreach (var wavefrontVertex in mesh.Indices)
        {
            var a = wavefrontVertex.Position + vertexCount;
            var b = wavefrontVertex.TextureCoordinate + vertexCount;
            var c = wavefrontVertex.Normal + vertexCount;

            // experiment with this
            // sb.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c + @"/" + c + @"/" + c + "\r\n");
            sb.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c + @"/" + c + @"/" + c + "\r\n");
        }

        return sb.ToString();
    }

    // ReSharper disable once UnusedMember.Local
    private static void BuildFaces(List<IMesh> meshes, StringBuilder sb, string materialName = "lambert3SG")
    {
        int currentIndexCount = 0;

        foreach (var mesh in meshes)
        {
            currentIndexCount++;
            sb.AppendLine("s off"); // smoothing groups off
            sb.AppendLine($"g Mesh_{mesh.Identity}");
            sb.AppendLine($"usemtl {materialName}");

            foreach (var wavefrontVertex in mesh.Indices)
            {
                var a = (ushort)(wavefrontVertex.Position + currentIndexCount);
                var b = (ushort)(wavefrontVertex.TextureCoordinate + currentIndexCount);
                var c = (ushort)(wavefrontVertex.Normal + currentIndexCount);

                sb.Append("f " + a + @"/" + a + @"/" + a + " " + b + @"/" + b + @"/" + b + " " + c + @"/" + c + @"/" + c + "\r\n");
            }
        }
    }

    // this needs to use different format not bitmaps
    private void SaveTextures(IMesh mesh, string meshName, string materialName = "lambert3SG")
    {
        if (mesh.Textures.Any())
        {
            for (var i = 0; i < mesh.Textures.Count; i++)
            {
                var texture = mesh.Textures[i];
                var asset = this.texturesArchive[texture.TextureId];

                using var image = mesh.Textures[i].TextureMap(asset!.Asset);
                var textureDirectory = this.ModelDirectory + "\\" + mesh.Identity.ToString(CultureInfo.InvariantCulture).Replace(" ", "_") + "_" + i + ".png";
                Log.Information($"TextureDirectory: {textureDirectory}");
                //using var fs = new FileStream(textureDirectory, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                image.SaveAsPng(textureDirectory);

                // TODO figure this shit out right here
                var usmtl = string.Format(USE_MATERIAL, "lambert3SG");
                if (this.indexMaterialDictionary[meshName] != string.Empty)
                {
                    // we're on more than one texture here ... shit
                    this.indexMaterialDictionary.Add($"{meshName}_{i}", textureDirectory);
                }
                else
                {
                    this.indexMaterialDictionary[meshName] = textureDirectory;
                }

                // this should go before each set of faces.
                // sb.AppendFormat(UseMaterial, $"Mesh_{mesh.Id}");
            }
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

    private void AppendMaterial(string mapName, string materialName)
    {
        this.material.AppendFormat(MATERIAL_NAME, materialName);
        this.material.Append(MATERIAL_WHITE);
        this.material.Append(MATERIAL_DIFFUSE);
        this.material.Append(MATERIAL_SPECULAR);
        this.material.Append(MATERIAL_SPECULAR_NS);
        this.material.Append(MATERIAL_DEFAULT_ILLUMINATION);
        this.material.AppendFormat(MAP_TO, mapName.Substring(mapName.LastIndexOf('\\') + 1));
    }
}