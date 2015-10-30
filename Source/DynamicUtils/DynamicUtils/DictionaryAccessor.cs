using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DynamicUtils
{
    public static class DictionaryAccessor
    {
        public static DictionaryAccessor<T> AsDictionary<T>(this T instance) => new DictionaryAccessor<T>(instance);
    }

    /// <summary>
    /// get/set properties through IDictionary with property names as a key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct DictionaryAccessor<T> : IDictionary<string, object>
    {
        private static readonly Func<T, string, object> _getter = ReflectionUtil.GetGetter<T>();
        private static readonly Action<T, string, object> _setter = ReflectionUtil.GetSetter<T>();
        private static readonly ICollection<string> _keys = new ReadOnlyCollection<string>(ReflectionUtil.GetPropertyNames(typeof(T)).ToArray());

        private readonly T _instance;

        public DictionaryAccessor(T instance) { _instance = instance; }

        public object this[string key]
        {
            get { return _getter(_instance, key); }
            set { _setter(_instance, key, value); }
        }

        int ICollection<KeyValuePair<string, object>>.Count => _keys.Count;

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => true;

        ICollection<string> IDictionary<string, object>.Keys => _keys;

        ICollection<object> IDictionary<string, object>.Values => this.Select(x => x.Value).ToArray();

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, object>.ContainsKey(string key) => _keys.Contains(key);

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var key in _keys)
            {
                yield return new KeyValuePair<string, object>(key, this[key]);
            }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            throw new NotImplementedException();
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value) 
        {
            if (!_keys.Contains(key))
            {
                value = default(T);
                return false;
            }

            value = this[key];
            return true;
        }
    }
}
