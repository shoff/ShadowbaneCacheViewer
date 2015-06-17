using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CacheViewer.Domain.Archive;

namespace CacheViewer.Domain.Collections
{
    public class CacheDictionary : IDictionary<CacheIndex, byte[]>
    {
        private readonly Dictionary<int, int> keyCounts = new Dictionary<int, int>();

        private readonly Dictionary<CacheIndex, byte[]> internalDictionary = new Dictionary<CacheIndex, byte[]>();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<CacheIndex, byte[]>> GetEnumerator()
        {
            return this.internalDictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<CacheIndex, byte[]> item)
        {
            if (keyCounts.ContainsKey(item.Key.identity))
            {
                keyCounts[item.Key.identity] = keyCounts[item.Key.identity] + 1;
                CacheIndex ci = new CacheIndex
                {
                    identity = item.Key.identity,
                    flag = item.Key.flag,
                    compressedSize = item.Key.compressedSize,
                    junk1 = item.Key.junk1,
                    name = item.Key.name,
                    offset = item.Key.offset,
                    // we -1 this so that our order starts at 0
                    order = keyCounts[item.Key.identity] - 1
                };
                this.internalDictionary.Add(ci, item.Value);
            }
            else
            {
                this.internalDictionary.Add(item.Key, item.Value);
            }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            this.internalDictionary.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<CacheIndex, byte[]> item)
        {
            return this.internalDictionary.ContainsKey(item.Key);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void CopyTo(KeyValuePair<CacheIndex, byte[]>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Remove(KeyValuePair<CacheIndex, byte[]> item)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get { return this.internalDictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(CacheIndex key)
        {
            return this.internalDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(CacheIndex key, byte[] value)
        {
            if (keyCounts.ContainsKey(key.identity))
            {
                keyCounts[key.identity] = keyCounts[key.identity] + 1;
                CacheIndex ci = new CacheIndex
                {
                    identity = key.identity,
                    flag = key.flag,
                    compressedSize = key.compressedSize,
                    junk1 = key.junk1,
                    name = key.name,
                    offset = key.offset,
                    // we -1 this so that our order starts at 0
                    order = keyCounts[key.identity] - 1
                };
                this.internalDictionary.Add(ci, value);
            }
            else
            {
                keyCounts.Add(key.identity, 1);
                this.internalDictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Remove(CacheIndex key)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool TryGetValue(CacheIndex key, out byte[] value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public byte[] this[CacheIndex key]
        {
            get { return GetItem(key); }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private byte[] GetItem(CacheIndex key)
        {
            var item = (from x in this.internalDictionary
                        where x.Key.identity == key.identity
                        select x.Value).FirstOrDefault();
            if (item != null)
            {
                return item.ToArray();
            }
            return new byte[0];
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<CacheIndex> Keys
        {
            get { return this.internalDictionary.Keys; }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<byte[]> Values
        {
            get { return this.internalDictionary.Values; }
        }
    }
}