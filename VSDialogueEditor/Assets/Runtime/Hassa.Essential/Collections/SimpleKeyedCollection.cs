using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Hassa.Essentials
{
    public class SimpleKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
    {

        private readonly Func<TItem, TKey> keySelector;

        public SimpleKeyedCollection(Func<TItem, TKey> _keySelector)
            : base()
        {
            Ensure.That(nameof(_keySelector)).IsNotNull(_keySelector);
            this.keySelector = _keySelector;
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return this.keySelector(item);
        }

        public virtual ReadOnlyKeyedCollection<TKey, TItem> AsReadOnly()
        {
            return new ReadOnlyKeyedCollection<TKey, TItem>(this);
        }

    }

}
