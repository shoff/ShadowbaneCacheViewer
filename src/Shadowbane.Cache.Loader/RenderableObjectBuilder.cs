namespace Shadowbane.Cache.Loader
{
    using Models;

    public class RenderableObjectBuilder
    {
#pragma warning disable IDE0032 // Use auto property
        private static readonly RenderableObjectBuilder instance = new RenderableObjectBuilder();
#pragma warning restore IDE0032 // Use auto property

        public static RenderableObjectBuilder Instance => instance;

        private RenderableObjectBuilder(){}

        public RenderInformation Create(in uint render, int i, bool b)
        {
            throw new System.NotImplementedException();
        }
    }
}