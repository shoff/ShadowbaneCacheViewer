namespace Shadowbane.Cache
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class InvalidMeshException : ApplicationException
    {
        public InvalidMeshException(string message)
            : base(message){}
        private InvalidMeshException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}