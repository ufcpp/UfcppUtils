using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Linq.Expressions.Expression;
using static System.Reflection.BindingFlags;

namespace DynamicUtils
{
    public class ReflectionUtil
    {
        public static IEnumerable<string> GetPropertyNames(Type t)
            =>
            from p in t.GetAllProperties()
            where p.GetGetMethod() != null
            select p.Name;

        /// <summary>
        /// get a delegate which gets a property value from a property name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, string, object> GetGetter<T>()
        {
            var t = typeof(T);

            var properties = (
                from p in t.GetAllProperties()
                where p.GetGetMethod() != null
                select p
                ).ToArray();

            if (!properties.Any()) return (x, y) => null;

            var instance = Parameter(t, "instance");
            var propertyName = Parameter(typeof(string), "propertyName");

            var cases = (
                from p in properties
                select SwitchCase(
                    Convert(Property(instance, p), typeof(object)),
                    Constant(p.Name)
                    )
                ).ToArray();

            var ex = Lambda<Func<T, string, object>>(
                Switch(
                    propertyName,
                    Constant(null, typeof(object)),
                    cases.ToArray()),
                instance,
                propertyName
                );

            return ex.Compile();
        }

        /// <summary>
        /// get a delegate which sets a property value from a property name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Action<T, string, object> GetSetter<T>()
        {
            var t = typeof(T);

            var properties = (
                from p in t.GetAllProperties()
                where p.GetSetMethod() != null
                select p
                ).ToArray();

            if (!properties.Any()) return (x, y, z) => { };

            var instance = Parameter(t, "instance");
            var propertyName = Parameter(typeof(string), "propertyName");
            var value = Parameter(typeof(object), "value");

            //System.Linq.Expressions.Expression.
            var cases =
                from p in properties
                select SwitchCase(
                    Block(typeof(void), Assign(Property(instance, p), Convert(value, p.PropertyType))),
                    Constant(p.Name)
                    );

            var ex = Lambda<Action<T, string, object>>(
                Switch(
                    propertyName,
                    cases.ToArray()),
                instance,
                propertyName,
                value
                );

            return ex.Compile();
        }
    }
}
