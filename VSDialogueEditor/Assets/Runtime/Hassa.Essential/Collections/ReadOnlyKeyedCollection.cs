using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hassa.Essentials
{
    public class ReadOnlyKeyedCollection<TKey, TValue> : ReadOnlyCollection<TValue>, IEnumerable
    {
        KeyedCollection<TKey, TValue> collection;

        public ReadOnlyKeyedCollection(KeyedCollection<TKey, TValue> _collection) : base(_collection) {
            Ensure.That(nameof(_collection)).IsNotNull(_collection);
            this.collection = _collection;
        }

        public TValue this[TKey key] {
            get => this.collection[key];
        }

        new public TValue this[int index] {
            get => this.collection[index];
        }

        public bool Contains(TKey key) {
            return this.collection.Contains(key);
        }
    }
}