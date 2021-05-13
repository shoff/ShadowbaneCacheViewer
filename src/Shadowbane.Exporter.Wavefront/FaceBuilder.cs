namespace Shadowbane.Exporter.Wavefront
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using ChaosMonkey.Guards;

    internal static class FaceBuilder
    {
        private static readonly ReadOnlyMemory<char> atSlash = "/".AsMemory();
        private static readonly ReadOnlyMemory<char> faceStart = "f ".AsMemory();

        private static IDictionary<string, string?> ToDictionary<TModel>(
            this PropertyDescriptorCollection propertyDescriptorCollection,
            TModel requestModel)
        {
            Guard.IsNotNull(requestModel, nameof(requestModel));
            var urlList = new Dictionary<string, string?>();

            foreach (PropertyDescriptor property in propertyDescriptorCollection)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                // ReSharper is being stupid, there's no way it could be null here.
                var value = property.GetValue(requestModel);

                if (value == null)
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(property.Name))
                {
                    continue;
                }

                // let's not let people put repetitive keys in the url
                if (urlList.ContainsKey(property.Name))
                {
                    continue;
                }
                urlList.Add(property.Name, value.ToString());
            }

            return urlList;
        }

        internal static ReadOnlyMemory<char> ToFace(this Index index)
        {
            Guard.IsNotNull(index, nameof(index));
            ReadOnlyMemory<char> mem = faceStart;
            var line = ToFaceEntry(mem, 
                ToFaceEntry(index.Position.ToString().AsMemory(), atSlash)
            // int i = 0;
            //foreach (var kvp in d)
            //{
            //    var keyValuePair = ToFaceEntry(ToFaceEntry(kvp.Key.AsMemory(), @equals), kvp.Value.AsMemory());
            //    mem = i == 0 ? keyValuePair : ToFaceEntry(mem, ToFaceEntry(and, keyValuePair));
            //    i++;
            //}
            return mem;
        }

        internal static unsafe ReadOnlyMemory<char> ToFaceEntry(
            ReadOnlyMemory<char> position, 
            ReadOnlyMemory<char> texture,
            ReadOnlyMemory<char> normal)
        {
            fixed (char* p = &MemoryMarshal.GetReference(position.Span),
                         t = &MemoryMarshal.GetReference(texture.Span),
                         n = &MemoryMarshal.GetReference(normal.Span))
            {
                var sLength = position.Length + texture.Length + normal.Length
                    + faceStart.Length + (atSlash.Length * 6) + 2;

                return string.Create(sLength, (
                        First: (IntPtr)p, FirstLength: position.Length,
                        Second: (IntPtr)t, SecondLength: texture.Length,
                        Third: (IntPtr)n, ThirdLength: normal.Length, false),

                    (destination, state) =>
                    {
                        new Span<char>((char*)state.First, state.FirstLength).CopyTo(destination);
                        new Span<char>((char*)state.Second, state.SecondLength).CopyTo(destination.Slice(state.FirstLength));
                    }).AsMemory();
            }
        }
    }
}