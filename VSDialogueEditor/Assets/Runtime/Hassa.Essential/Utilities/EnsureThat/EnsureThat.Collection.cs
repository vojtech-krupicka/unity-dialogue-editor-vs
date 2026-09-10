using System;
using System.Collections;
using System.Collections.Generic;

namespace Hassa.Essentials
{
    public partial class EnsureThat
    {

        public void HasKeyOf<TKey, TValue>(IDictionary<TKey, TValue> value, TKey expectedKey, string keyLabel = null)
        {
            IsNotNull(value);

            if (!value.ContainsKey(expectedKey)) {
                throw new ArgumentException($"{expectedKey} '{keyLabel ?? paramName}' was not found.", paramName);
            }
        }

    }
}