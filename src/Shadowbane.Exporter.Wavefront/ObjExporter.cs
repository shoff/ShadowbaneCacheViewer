namespace Shadowbane.Exporter.Wavefront;

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChaosMonkey.Guards;
using Geometry;
using Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

public class ObjExporter
{
    private const string SB_RENDER_ID = "# RenderId: {0}\r\n";
    private const string MATERIAL_LIB = "mtllib {0}.mtl\r\n";
    private const string VERTICE = "v {0} {1} {2}\r\n";
    private const string NORMAL = "vn {0} {1} {2}\r\n";
    private const string TEXTURE = "vt {0} {1}\r\n";
    private const string MATERIAL_NAME = "newmtl {0}\r\n";
    private const string GROUP_NAME = "g {0}\r\n";
    private const string MATERIAL_WHITE = "Ka 1.000 1.000 1.000\r\n";
    private const string MATERIAL_DIFFUSE = "Kd 1.000 1.000 1.000\r\n";
    private const string MATERIAL_SPECULAR = "Ks 0.000 0.000 0.000\r\n ";
    private const string MATERIAL_SPECULAR_NS = "Ns 10.000";
    private const string MATERIAL_DEFAULT_ILLUMINATION = "illum 2\r\n";
    private const string MAP_TO = "map_Ka {0}\r\nmap_Kd {0}\r\nmap_Ks {0}\r\n";
    private static readonly JpegEncoder jpegEncoder = new JpegEncoder();

        
    public static async Task ExportAsync(Renderable cacheObject, string outputDirectory, CancellationToken cancellationToken)
    {
        Guard.IsNotNull(cacheObject, nameof(cacheObject));
        var mainStringBuilder = new StringBuilder();
        var materialBuilder = new StringBuilder();

        // todo - not all objects seem to have names
        var name = CreateName(cacheObject);

        var exportDirectory = EnsureDirectory(name, outputDirectory);
        mainStringBuilder.Append(MayaObjHeaderFactory.Instance.Create((int) cacheObject.Identity));
        mainStringBuilder.AppendFormat(MATERIAL_LIB, name);

        // save the obj
        using var fs = new FileStream($"{exportDirectory}\\{name}.obj", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        using var writer = new StreamWriter(fs);
        writer.Write(mainStringBuilder.ToString());
            
        // save the material
        var mtlFile = $"{exportDirectory}\\{name}.mtl";
        if (File.Exists(mtlFile))
        {
            File.Delete(mtlFile);
        }

        using var fs1 = new FileStream(mtlFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        using var writer1 = new StreamWriter(fs1);
        await writer1.WriteAsync(materialBuilder.ToString());
    }

    private static string CreateName(Renderable cacheObject)
    {
        var name = cacheObject.Identity + "_";
        return name;
    }

    public void CreateObject(string name, Mesh mesh, StringBuilder mainStringBuilder,
        StringBuilder materialBuilder, string directory)
    {
        if (mesh == null)
        {
            throw new ArgumentNullException(nameof(mesh));
        }

        if (mainStringBuilder == null)
        {
            throw new ArgumentNullException(nameof(mainStringBuilder));
        }

        if (materialBuilder == null)
        {
            throw new ArgumentNullException(nameof(materialBuilder));
        }

        if (string.IsNullOrWhiteSpace(directory))
        {
            throw new ArgumentNullException(nameof(directory));
        }

        mainStringBuilder.AppendFormat(SB_RENDER_ID, mesh.Identity);
        var mapFiles = new List<string>();
            
        if (mesh.Textures.Any())
        {
            //var archive = (Textures)ArchiveFactory.Instance.Build(CacheFile.Textures);
            for (var i = 0; i < mesh.Textures.Count; i++)
            {
                // var texture = mesh.Textures[i];
                //var asset = ArchiveLoader.
                //    archive[texture.TextureId];
                //using var map = mesh.Textures[i].TextureMap(asset.Item1);
                using var map = mesh.Textures[i].Image;
                var mapName = $"{directory}\\{mesh.Identity.ToString(CultureInfo.InvariantCulture).Replace(" ", "_")}_{i}.jpg";
                mapFiles.Add(mapName);
                map.SaveAsJpeg(mapName, jpegEncoder);
            }
        }

        // for now let's just see if we can even get one to work
        if (mapFiles.Count > 0)
        {
            this.CreateMaterial(name, mapFiles[0], materialBuilder);
        }

        foreach (var v in mesh.Vertices)
        {
            mainStringBuilder.AppendFormat(VERTICE, v[0], v[1], v[2]);
        }

        foreach (var vn in mesh.Normals)
        {
            mainStringBuilder.AppendFormat(NORMAL, vn[0], vn[1], vn[2]);
        }

        foreach (var t in mesh.TextureVectors)
        {
            mainStringBuilder.AppendFormat(TEXTURE, t[0], t[1]);
        }

        for (var i = 0; i < mesh.Indices.Count; i++)
        {
            var a = (ushort)(mesh.Indices[i].Position + 1);
            var b = (ushort)(mesh.Indices[i].TextureCoordinate + 1);
            var c = (ushort)(mesh.Indices[i].Normal + 1);

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
    //                            # be the same as the ambient texture map)
    // map_Ks lenna.tga           # the specular texture map
    // map_d lenna_alpha.tga      # the alpha texture map
    // map_bump lenna_bump.tga    # the bump map
    // bump lenna_bump.tga        # some implementations use 'bump' instead of 'map_Bump'
    private void CreateMaterial(string name, string mapName, StringBuilder materialBuilder)
    {
        materialBuilder.AppendFormat(MATERIAL_NAME, name);
        materialBuilder.Append(MATERIAL_WHITE);
        materialBuilder.Append(MATERIAL_DIFFUSE);
        materialBuilder.Append(MATERIAL_SPECULAR);
        materialBuilder.Append(MATERIAL_SPECULAR_NS);
        materialBuilder.Append(MATERIAL_DEFAULT_ILLUMINATION);
        materialBuilder.AppendFormat(MAP_TO, mapName.Substring(mapName.LastIndexOf('\\') + 1));
    }

    private static string EnsureDirectory(string name, string modelDirectory)
    {
        var fullName = Path.Combine(modelDirectory, name);

        if (Directory.Exists(fullName))
        {
            Directory.Delete(fullName, true);
        }

        Directory.CreateDirectory(fullName);
        return fullName;
    }


}