using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CacheViewer.Domain.Exporters;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;

namespace CacheViewer.Controls.Viewer
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjLoader
    {
        /// <summary>
        /// 
        /// </summary>
        public enum NormalLoad
        {
            /// <summary>
            /// The ignore
            /// </summary>
            Ignore,
            /// <summary>
            /// The load
            /// </summary>
            Load,
            /// <summary>
            /// The generate surface
            /// </summary>
            GenerateSurface,
            /// <summary>
            /// The generate vertex
            /// </summary>
            GenerateVertex
        }

        /// <summary>
        /// Froms the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="file">The file.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="technique">The technique.</param>
        /// <param name="normalLoad">The normal load.</param>
        /// <returns></returns>
        public static List<DisplayMesh> FromFile(string directory, string file, Effect effect, string technique, NormalLoad normalLoad)
        {
            using (var stream = new FileStream(directory + "\\" + file, FileMode.Open))
            {
                return FromStream(stream, directory, effect, technique, normalLoad);
            }
        }

        /// <summary>
        /// Froms the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="directory">The directory.</param>
        /// <param name="effect">The effect.</param>
        /// <param name="technique">The technique.</param>
        /// <param name="inNormalLoad">The in normal load.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        public static List<DisplayMesh> FromStream(Stream stream, string directory, Effect effect, string technique,
            NormalLoad inNormalLoad)
        {
            List<DisplayMesh> output = new List<DisplayMesh>();
           
            using (TextReader reader = new StreamReader(stream))
            {
                while (reader.Peek() != 'm')
                {
                    reader.ReadLine();
                }

                string mtlLib = reader.ReadLine();

                if (!mtlLib.StartsWith("mtllib "))
                {
                    throw new InvalidDataException(
                        string.Format("Se esperaba \"mtllib materialfile\", en lugar de \"'{0}'\".", mtlLib));
                }

                Dictionary<string, Texture2D> materials = ParseMtllib(directory, mtlLib.Substring(mtlLib.IndexOf(' ') + 1));

                // to GenerateVertex
                List<Vector3> vertAux = new List<Vector3>();
                int indexOffsetTotal = 0;

                string modelName = "Unnamed";
                string line = "";
                List<Vector3> verticesTotales = new List<Vector3>();
                List<Vector2> texcoordsTotales = new List<Vector2>();
                List<Vector3> normalsTotales = new List<Vector3>();
                Texture2D texActual = null;
                


                // Peek = -1 -> EOF
                while (reader.Peek() >= 0)
                {
                    bool newModel = true;

                    // Scroll to the name (if any) or vertices
                    while (!(line.StartsWith("o ") || line.StartsWith("g ") || line.StartsWith("v ") ||
                          line.StartsWith("vt ") || line.StartsWith("vn ") || line.StartsWith("usemtl")))
                    {
                        line = ReadLineNotEmpty(reader);
                    }

                    while (line.StartsWith("o ") || line.StartsWith("g ") || line.StartsWith("v ") ||
                           line.StartsWith("vt ") || line.StartsWith("vn ") || line.StartsWith("usemtl"))
                    {
                        string[] fields = line.Split(new[]
                        {
                            ' '
                        }, StringSplitOptions.RemoveEmptyEntries);
                        switch (fields[0])
                        {
                            // or object name
                            case "o":
                            // group nbamne
                            case "g":
                                modelName = fields[1];
                                if (modelName.StartsWith("Mesh.156"))
                                {
                                    int a = 3;
                                }
                                break;
                            // 
                            case "v":
                                if (inNormalLoad == NormalLoad.GenerateVertex)
                                {
                                    if (newModel)
                                    {
                                        indexOffsetTotal += vertAux.Count;
                                        vertAux = new List<Vector3>();
                                        newModel = false;
                                    }
                                    vertAux.Add(new Vector3(ReadFloat(fields[1]), ReadFloat(fields[2]),
                                        ReadFloat(fields[3])));
                                }
                                else
                                {
                                    verticesTotales.Add(new Vector3(ReadFloat(fields[1]), ReadFloat(fields[2]),
                                        ReadFloat(fields[3])));
                                }
                                break;
                            //vt TextureCoordinate.x TextureCoordinate.y
                            case "vt":
                                texcoordsTotales.Add(new Vector2(ReadFloat(fields[1]), -ReadFloat(fields[2])));
                                break;
                            //vn Normal.x Normal.y Normal.z
                            case "vn":
                                normalsTotales.Add(new Vector3(ReadFloat(fields[1]), ReadFloat(fields[2]),
                                    ReadFloat(fields[3])));
                                break;
                            // usemtl material
                            case "usemtl":
                                materials.TryGetValue(fields[1], out texActual);
                                break;
                        }
                        line = ReadLineNotEmpty(reader);
                    }

                    if (texActual != null)
                    {
                        MeshData meshData
                            ;
                        if (inNormalLoad == NormalLoad.GenerateVertex)
                        {
                            meshData = GenerarNormalesVertex(vertAux, texcoordsTotales, indexOffsetTotal, ref line, reader);
                        }
                        else
                        {
                            meshData = LeerVertex(verticesTotales, texcoordsTotales, normalsTotales, inNormalLoad, ref line,
                                reader);
                        }
                        output.Add(new DisplayMesh(effect, technique, meshData, texActual, defaultName:modelName));
                    }
                    else
                    {
                        // Skip long faces for not having the texture
                        while (!line.StartsWith("f"))
                        {
                            line = ReadLineNotEmpty(reader);
                        }
                        while (reader.Peek() >= 0 && line.StartsWith("f "))
                        {
                            line = ReadLineNotEmpty(reader);
                        }
                    }
                }
                return output;
            }
        }

        /// <summary>
        /// Generars the normales vertex.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <param name="texcoordsTotales">The texcoords totales.</param>
        /// <param name="indexOffset">The index offset.</param>
        /// <param name="line">The line.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private static MeshData GenerarNormalesVertex(List<Vector3> vertices, List<Vector2> texcoordsTotales,
            int indexOffset, ref string line, TextReader reader)
        {
            MeshData meshAux = new MeshData();
            meshAux.Positions = new List<Vector3>(vertices);
            int cantVertices = vertices.Count;
            int[] indAbsolutoTex = new int[cantVertices];
            HashSet<Vector3>[] normalesPorVertice = new HashSet<Vector3>[cantVertices];
            for (int i = 0; i < cantVertices; i++)
            {
                normalesPorVertice[i] = new HashSet<Vector3>();
            }

            //Avanzar hasta f
            while (!line.StartsWith("f"))
            {
                line = ReadLineNotEmpty(reader);
            }
            //f IndexVer1/IndexTex1/IndexNor1 IndexVer2/IndexTex2/IndexNor2 IndexVer3/IndexTex3/IndexNor3
            while (reader.Peek() >= 0 && line.StartsWith("f "))
            {
                Vector3[] verticesFace = new Vector3[3];
                string[] indicesFaces = new string[3];
                string[] fields = line.Split(new[]
                {
                    ' '
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < 4; i++)
                {
                    //string[] indicesFace = fields[i].Split(new[]
                    //{
                    //    '/'
                    //}, StringSplitOptions.None);
                    //indicesFaces[i - 1] = indicesFace[0];
                    //meshAux.Indices.Add(ReadInt(indicesFace[0]) - 1 - indexOffset);
                    //indAbsolutoTex[ReadInt(indicesFace[0]) - 1 - indexOffset] = ReadInt(indicesFace[1]) - 1;
                    //verticesFace[i - 1] = meshAux.Positions[ReadInt(indicesFace[0]) - 1 - indexOffset];
                }

                Vector3 normal =
                    Vector3.Normalize(Vector3.Cross(verticesFace[2] - verticesFace[1], verticesFace[0] - verticesFace[1]));
                // Different normal for the same vertex => average
                normalesPorVertice[ReadInt(indicesFaces[0]) - 1 - indexOffset].Add(normal);
                normalesPorVertice[ReadInt(indicesFaces[1]) - 1 - indexOffset].Add(normal);
                normalesPorVertice[ReadInt(indicesFaces[2]) - 1 - indexOffset].Add(normal);
                line = reader.ReadLine();
            }
            for (int i = 0; i < meshAux.Positions.Count; i++)
            {
                meshAux.TextureCoordinates.Add(texcoordsTotales[indAbsolutoTex[i]]);
                Vector3 normal = Vector3.Zero;
                for (int j = 0; j < normalesPorVertice[i].Count; j++)
                {
                    normal += normalesPorVertice[i].ElementAt(j);
                }
                normal /= normalesPorVertice[i].Count;
                meshAux.Normals.Add(normal);
            }
            return meshAux;
        }

        /// <summary>
        /// Leers the vertex.
        /// </summary>
        /// <param name="verticesTotales">The vertices totales.</param>
        /// <param name="texcoordsTotales">The texcoords totales.</param>
        /// <param name="normalsTotales">The normals totales.</param>
        /// <param name="inNormalLoad">The in normal load.</param>
        /// <param name="line">The line.</param>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private static MeshData LeerVertex(List<Vector3> verticesTotales, List<Vector2> texcoordsTotales,
            List<Vector3> normalsTotales, NormalLoad inNormalLoad, ref string line, TextReader reader)
        {
            string[] fields;
            string[] indicesFace;
            MeshData meshAux = new MeshData();
            int index = 0;

            // Advance to f
            while (!line.StartsWith("f"))
            {
                line = ReadLineNotEmpty(reader);
            }


            // IndexVer1 f / IndexTex1 / IndexNor1 IndexVer2 / IndexTex2 / IndexNor2 IndexVer3 / IndexTex3 / IndexNor3
            while (reader.Peek() >= 0 && line.StartsWith("f "))
            {
                fields = line.Split(new[]
                {
                    ' '
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < 4; i++)
                {
                    //indicesFace = fields[i].Split(new[]
                    //{
                    //    '/'
                    //}, StringSplitOptions.None);

                    //meshAux.Positions.Add(verticesTotales[ReadInt(indicesFace[0]) - 1]);
                    //meshAux.TextureCoordinates.Add(texcoordsTotales[ReadInt(indicesFace[1]) - 1]);
                    //if (inNormalLoad == NormalLoad.Load)
                    //{
                    //    meshAux.Normals.Add(normalsTotales[ReadInt(indicesFace[2]) - 1]);
                    //}
                    //meshAux.Indices.Add(index++);
                }
                if (inNormalLoad == NormalLoad.GenerateSurface)
                {
                    Vector3 v1 = meshAux.Positions[index - 3];
                    Vector3 v2 = meshAux.Positions[index - 2];
                    Vector3 v3 = meshAux.Positions[index - 1];
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(v3 - v2, v1 - v2));
                    meshAux.Normals.Add(normal);
                    meshAux.Normals.Add(normal);
                    meshAux.Normals.Add(normal);
                }
                line = ReadLineNotEmpty(reader);
            }
            return meshAux;
        }

        /// <summary>
        /// Parses the mtllib.
        /// </summary>
        /// <param name="inDir">The in dir.</param>
        /// <param name="inMatPath">The in mat path.</param>
        /// <returns></returns>
        private static Dictionary<string, Texture2D> ParseMtllib(string inDir, string inMatPath)
        {
            Dictionary<string, Texture2D> retorno = new Dictionary<string, Texture2D>();
            using (var stream = new FileStream(inDir + "\\" + inMatPath, FileMode.Open))
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while (reader.Peek() >= 0)
                    {
                        while (reader.Peek() >= 0 && !line.StartsWith("newmtl"))
                        {
                            line = reader.ReadLine();
                        }
                        if (reader.Peek() < 0)
                        {
                            break;
                        }
                        string name = line.Substring(line.IndexOf(' ') + 1);
                        line = reader.ReadLine();
                       /* If the material is textured, ignore it */
                        while (reader.Peek() >= 0 && !(line.StartsWith("map_Kd") || line.StartsWith("newmtl")))
                        {
                            line = reader.ReadLine();
                        }

                        // this is so hackish
                        if (line.StartsWith("map_Kd"))
                        {
                            string texture = line.Substring(line.IndexOf(' ') + 1);
                            var format = ImageLoadInformation.FromDefaults();
                            format.Format = (texture.EndsWith(".jpg")) ? Format.B8G8R8X8_UNorm : Format.R8G8B8A8_UNorm;
                            Texture2D tex;
                            try
                            {
                                tex = Texture2D.FromFile(ViewerBase.Instance.ViewDevice, inDir + "\\" + texture, format);
                            }
                            catch
                            {
                                tex = null;
                            }
                            if (tex != null)
                            {
                                retorno.Add(name, tex);
                            }
                        }
                        if (reader.Peek() < 0)
                        {
                            break;
                        }
                    }
                }
            }
            return retorno;
        }

        /// <summary>
        /// Reads the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        private static int ReadInt(string value)
        {
            int result;
            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                throw new InvalidDataException(string.Format("Expected an integer value, but found '{0}' instead.",
                    value));
            }

            return result;
        }

        /// <summary>
        /// Reads the float.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        private static float ReadFloat(string value)
        {
            float result;
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                throw new InvalidDataException(string.Format(
                    "Expected a floating point value, but found '{0}' instead.", value));
            }

            return result;
        }

        /// <summary>
        /// Reads the line not empty.
        /// </summary>
        /// <param name="inReader">The in reader.</param>
        /// <returns></returns>
        private static string ReadLineNotEmpty(TextReader inReader)
        {
            string s = inReader.ReadLine();
            while (s == "")
            {
                s = inReader.ReadLine();
            }
            return s;
        }
    }
}