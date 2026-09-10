using System;
using System.Collections;
using System.Collections.Generic;

namespace Hassa.Essentials
{
    public class ReadOnlyDictionaryEx<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> m_collection;

        public ReadOnlyDictionaryEx(IDictionary<TKey, TValue> collection)
        {
            Ensure.That(nameof(collection)).IsNotNull(collection);
            m_collection = collection;
        }

        public TValue this[TKey key] {
            get => m_collection[key];
        }

        TValue IDictionary<TKey, TValue>.this[TKey key] {
            get => m_collection[key];
            set => throw new NotSupportedException();
        }

        public ICollection<TKey> Keys => m_collection.Keys;

        public ICollection<TValue> Values => m_collection.Values;

        public int Count => m_collection.Count;

        public bool IsReadOnly => true;

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return m_collection.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return m_collection.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            m_collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_collection.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_collection.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_collection.GetEnumerator();
        }
    }
}