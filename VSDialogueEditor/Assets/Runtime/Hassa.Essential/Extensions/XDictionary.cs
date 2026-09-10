using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hassa.Essentials
{
    public static class XDictionary
    {

        /// <summary>
        /// Get key from dictionary. If key is not exists, return default value.
        /// </summary>
        /// <param name="dictionary">A dictionary.</param>
        /// <param name="key">A key.</param>
        /// <param name="defaultValue">Default value, which will be returned if key not exists within dictionary.</param>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <returns>TValue at given key or default.</returns>
        public static TValue Get<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default)
        {
            return key == null || !dictionary.TryGetValue(key, out var value)
                ? defaultValue
                : value;
        }

        /// <summary>
        /// Append item at key, where value is IList.
        /// </summary>
        /// <param name="dictionary">A dictionary.</param>
        /// <param name="key">A key.</param>
        /// <param name="value">Value to append to list.</param>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value in list.</typeparam>
        public static void AppendItem<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key)) {
                dictionary[key].Add(value);
                return;
            }

            dictionary.Add(key, new List<TValue>() { value });
        }

        /// <summary>
        /// Create new ReadOnlyDictionary wrapper around given dictionary.
        /// </summary>
        /// <param name="dictionary">A dictionary to wrap.</param>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <returns>Returns ReadnOnlyDictionary.</returns>
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}