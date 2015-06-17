using System.Diagnostics.Contracts;
using SlimDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CacheViewer.Domain.Parsers
{

    /// <summary>
    ///     Class for reading a 3D mesh in the Wavefront OBJ format from a stream.
    /// </summary>
    public class WavefrontReader
    {
        /// <summary>
        /// Enum for describing the semantic meaning of a line in an OBJ file.
        /// </summary>
        private enum DataType
        {
            /// <summary>
            /// The line contains nothing or has no or an undefined keyword.
            /// </summary>
            Empty,

            /// <summary>
            /// The line contains a comment.
            /// </summary>
            Comment,

            /// <summary>
            /// The line contains a group definition.
            /// </summary>
            Group,

            /// <summary>
            /// The line contains a smoothing group definitio.
            /// </summary>
            SmoothingGroup,

            /// <summary>
            /// The line contains a position vector definition.
            /// </summary>
            Position,

            /// <summary>
            /// The line contains a normal vector definition.
            /// </summary>
            Normal,

            /// <summary>
            /// The line contains a texture coordinate definition.
            /// </summary>
            TexCoord,

            /// <summary>
            /// The line contains a face definition.
            /// </summary>
            Face,
        }

        private static Dictionary<DataType, string> Keywords
        {
            get
            {
                return new Dictionary<DataType, string>
                {
                    { DataType.Comment,         "#"     },
                    { DataType.Group,           "g"     },
                    { DataType.SmoothingGroup,  "s"     },
                    { DataType.Position,        "v"     },
                    { DataType.TexCoord,        "vt"    },
                    { DataType.Normal,          "vn"    },
                    { DataType.Face,            "f"     },
                };
            }
        }

        /// <summary>
        ///     Reads a WavefrontObject instance from the stream.
        /// </summary>
        /// <param name="stream">
        ///     Stream containing the OBJ file content.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stream"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="IOException">
        ///     Error while reading from the stream.
        /// </exception>
        public WavefrontObject Read(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null);
            Contract.Ensures(Contract.Result<WavefrontObject>() != null);

            // Create the stream reader for the file
            var reader = new StreamReader(stream);

            // Store the lines here
            var lines = new List<string>();

            // Store the current line here
            string current;

            // Read the file line by line and normalize them
            while ((current = reader.ReadLine()) != null)
            {
                lines.Add(NormalizeLine(current));
            }

            // Create empty mesh instance
            var obj = new WavefrontObject();

            // Iterate over all lines
            foreach (string line in lines)
            {
                // Get line type and content
                DataType type = GetType(line);
                string content = GetContent(line, type);

                // Line is a position
                if (type == DataType.Position)
                {
                    obj.Positions.Add(ParseVector3(content));
                }

                // Line is a texture coordinate
                if (type == DataType.TexCoord)
                {
                    obj.Texcoords.Add(ParseVector2(content));
                }

                // Line is a normal vector
                if (type == DataType.Normal)
                {
                    obj.Normals.Add(ParseVector3(content));
                }

                // Line is a mesh sub group
                if (type == DataType.Group)
                {
                    obj.Groups.Add(new WavefrontFaceGroup { Name = content });
                }

                // Line is a polygon
                if (type == DataType.Face)
                {
                    // Create the default group for all faces outside a group
                    if (obj.Groups.Count == 0)
                    {
                        obj.Groups.Add(new WavefrontFaceGroup());
                    }

                    // Add the face to the last group added
                    obj.Groups.Last().Faces.Add(ParseFace(content));
                }
            }

            return obj;
        }

        private string NormalizeLine(string line)
        {
            // Trim beginning and end and collapse all whitespace in a string to single space.
            return System.Text.RegularExpressions.Regex.Replace(line.Trim(), @"\s+", " ");
        }

        private DataType GetType(string line)
        {
            // Get the type of data stored in the specified line.
            // Iterate over the keywords
            foreach (var item in Keywords)
            {
                var type = item.Key;
                var keyword = item.Value;

                // Line starts with current keyword
                if (line.ToLower().StartsWith(keyword.ToLower() + " "))
                {
                    // Return current type
                    return type;
                }
            }

            // No type
            return DataType.Empty;
        }

        private string GetContent(string line, DataType type)
        {
            // Remove the keyword from the start of the line and return the result.
            // Returns an empty string if the specified type was DataType.Empty.
            // If empty return empty string,
            // else remove the keyword from the start
            return type == DataType.Empty
                ? string.Empty
                : line.Substring(Keywords[type].Length).Trim();
        }

        internal static float[] ParseFloatArray(string str, int count)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(str));
            Contract.Requires<ArgumentException>(count > 0);


            // Create an array of floats of arbitary length from a string representation,
            // where the floats are spearated by whitespace.
            var floats = new float[count];

            var segments = str.Split(' ');

            for (int i = 0; i < count; i++)
            {
                if (i < segments.Length)
                {
                    try
                    {
                        floats[i] = float.Parse(segments[i], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        floats[i] = 0;
                    }
                }
            }

            return floats;
        }

        internal Vector2 ParseVector2(string str)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(str));
            // Parse a 3D vector from a string definition in the form of: 2.0 3.0 1.0
            var components = ParseFloatArray(str, 3);
            if (components.Length != 3)
            {
                return Vector2.Zero;
            }

            var vec = new Vector2(components[0], components[1]);
            return Math.Abs(components[2]) < .00001f ? vec : vec / components[2];
        }

        internal Vector3 ParseVector3(string str)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(str));
            // Parse a 3D vector from a string definition in the form of: 1.0 2.0 3.0 1.0
            var components = ParseFloatArray(str, 4);
            var vec = new Vector3(components[0], components[1], components[2]);
            return Math.Abs(components[3]) < .00001f ? vec : vec / components[3];
        }

        internal WavefrontFace ParseFace(string str)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(str));
            Contract.Ensures(Contract.Result<WavefrontFace>() != null);
            // Parse a OBJ face from a string definition.
            // Split the face definition at whitespace
            var segments = str.Split(new Char[0], StringSplitOptions.RemoveEmptyEntries);

            var vertices = new List<WavefrontVertex>();

            // Iterate over the segments
            foreach (string segment in segments)
            {
                // Parse and add the vertex
                vertices.Add(ParseVertex(segment));
            }

            // Create and return the face
            return new WavefrontFace
            {
                Vertices = vertices,
            };
        }

        internal WavefrontVertex ParseVertex(string str)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(str));
            Contract.Ensures(Contract.Result<WavefrontVertex>() != null);
            // Parse an OBJ vertex from a string definition in the forms of: 
            //     1/2/3
            //     1//3
            //     1/2
            //     1

            // Split the string definition at the slash separator
            var segments = str.Split('/');

            // Store the vertex indices here
            var indices = new int[3];

            // Iterate 3 times
            for (int i = 0; i < 3; i++)
            {
                // If no segment exists at the location or the segment can not be passed to an integer set the index to zero
                if ((segments.Length <= i) || (!int.TryParse(segments[i], out indices[i])))
                {
                    indices[i] = 0;
                }
            }

            // Create the new vertex
            return new WavefrontVertex(indices[0], indices[1], indices[2]);

        }
    }
}