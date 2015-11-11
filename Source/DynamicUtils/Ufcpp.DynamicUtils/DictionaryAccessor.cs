using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace DynamicUtils
{
    /// <summary>
    /// static/extension methods for <see cref="DictionaryAccessor{T}"/>.
    /// </summary>
    public static class DictionaryAccessor
    {
        /// <summary>
        /// create <see cref="DictionaryAccessor{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static DictionaryAccessor<T> AsDictionary<T>(this T instance) => new DictionaryAccessor<T>(instance);

        /// <summary>
        /// create <see cref="DictionaryAccessor{T}"/> by <see cref="Activator.CreateInstance(Type, object[])"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static object AsDictionary(object instance)
        {
            var t = instance.GetType();
            var openType = typeof(DictionaryAccessor<>);
            var closedType = openType.MakeGenericType(t);

            return Activator.CreateInstance(closedType, instance);
        }

    }

    internal interface IAccessor
    {
        object Instance { get; }
    }

    /// <summary>
    /// get/set properties through IDictionary with property names as a key.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct DictionaryAccessor<T> : IDictionary<string, object>, IAccessor
    {
        private static readonly Func<T, string, object> _getter = ReflectionUtil.GetGetter<T>();
        private static readonly Action<T, string, object> _setter = ReflectionUtil.GetSetter<T>();
        private static readonly ICollection<string> _keys = new ReadOnlyCollection<string>(ReflectionUtil.GetPropertyNames(typeof(T)).ToArray());

        private readonly T _instance;

        object IAccessor.Instance => _instance;

        public DictionaryAccessor(T instance) { _instance = instance; }

        public object this[string key]
        {
            get
            {
                var value = _getter(_instance, key);

                if (IsNativeType(value)
                    )
                    return value;

                return DictionaryAccessor.AsDictionary(value);
            }
            set
            {
                var accessor = value as IAccessor;

                if (accessor != null)
                {
                    _setter(_instance, key, accessor.Instance);
                }
                else
                {
                    _setter(_instance, key, value);
                }
            }
        }

        private static bool IsNativeType(object value) => value == null || IsNativeType(value.GetType());

        private static bool IsNativeType(Type t)
        {
            if (t.IsArray)
            {
                return IsNativeType(t.GetElementType());
            }
            else if (t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return IsNativeType(t.GetGenericArguments()[0]);
            }

            return t.GetTypeInfo().IsPrimitive
                || t == typeof(decimal)
                || t == typeof(string)
                || t == typeof(DateTime)
                || t == typeof(DateTimeOffset);
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
