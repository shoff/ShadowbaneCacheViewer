namespace Shadowbane.Cache.IO
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class ParseException : Exception
    {
        public ParseException(string parseErrorMessage)
            : base(parseErrorMessage)
        {
        }
        
        private ParseException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}