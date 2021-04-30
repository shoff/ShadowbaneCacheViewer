namespace Shadowbane.Exporter.Wavefront
{
    using System.Collections.Generic;

    public class WavefrontFaceGroup
    {
        public string Name { get; set; }
        public List<WavefrontFace> Faces { get; set; } = new List<WavefrontFace>();
    }
}