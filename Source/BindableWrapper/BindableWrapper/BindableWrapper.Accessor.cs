using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace BindableHelper
{
    public partial class BindableWrapper<T>
    {
        delegate object Getter(ref T obj);
        delegate void Setter(ref T obj, object value);

        struct Prop
        {
            public Getter Get;
            public Setter Set;
            public PropertyChangedEventArgs Arg;

            public Prop(Getter get, Setter set, PropertyChangedEventArgs arg) => (Get, Set, Arg) = (get, set, arg);
            public void Deconstruct(out Getter get, out Setter set, out PropertyChangedEventArgs arg) => (get, set, arg) = (Get, Set, Arg);
        }

        private static Dictionary<string, Prop> _accessors = new Dictionary<string, Prop>();

        private static void InitializeAccessors()
        {
            foreach (var m in typeof(T).GetProperties())
            {
                if (m.GetSetMethod() == null) continue; // set 可能なやつだけ
                var name = m.Name;
                _accessors.Add(name, InitializeAccessor(name, m.PropertyType, m));
            }

            foreach (var m in typeof(T).GetFields())
            {
                var name = m.Name;
                _accessors.Add(name, InitializeAccessor(name, m.FieldType, m));
            }
        }

        private static Prop InitializeAccessor(string name, Type memberType, MemberInfo m)
        {
            var obj = Parameter(typeof(T).MakeByRefType());

            // return obj.M
            var getter = Lambda<Getter>(
                Convert(
                    MakeMemberAccess(obj, m),
                    typeof(object)),
                obj).Compile();

            // obj.M = value;
            var value = Parameter(typeof(object));
            var setter = Lambda<Setter>(
                Assign(
                    MakeMemberAccess(obj, m),
                    Convert(value, memberType)),
                obj, value).Compile();

            var t = new Prop(getter, setter, new PropertyChangedEventArgs(name));
            return t;
        }
    }
}
