namespace Shadowbane.CacheViewer.Exceptions;

using System;

[Serializable]
public sealed class ModelDirectoryNotSetException : ApplicationException
{
    public ModelDirectoryNotSetException(string modelName)
        : base(
            $"The ModelDirectory property was not set for the model {modelName} prior to attempting to save the textures. Please set the property to an existing directory and try again.")
    { }

}