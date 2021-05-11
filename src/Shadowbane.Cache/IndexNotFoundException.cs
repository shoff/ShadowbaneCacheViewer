namespace Shadowbane.Cache
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class IndexNotFoundException : ApplicationException
    {

        public IndexNotFoundException(Type type, uint indexId)
            : base($"{type.Name} could not find a CacheIndex for identity {indexId}")
        {
        }

        public IndexNotFoundException(string name, uint indexId)
            : base($"{name} could not find a CacheIndex for identity {indexId}")
        {
        }

        private IndexNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            :base(serializationInfo, streamingContext)
        {
        }
    }

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