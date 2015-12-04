using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.Reflection.BindingFlags;

namespace DynamicUtils
{
    /// <summary>
    /// <see cref="Type"/>の拡張メソッド。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 基底クラスも含めて全部のフィールドを取得する。
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetAllFields(this Type t) => t.GetFields(Public | Instance).Where(f => !f.IsStatic && (f.Attributes & FieldAttributes.SpecialName) == 0);

        /// <summary>
        /// 基底クラスも含めて全部のプロパティを取得する。
        /// ただし、~Ignore って名前の属性を付けているものは無視。
        /// 今のところ、「~Ignore 」属性は名前でだけ判断。Ignoreさえ付けばどんな属性でも無視。
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type t) => t.GetProperties(Public | Instance).Where(p => !p.GetCustomAttributes().Any(a => a.GetType().Name.Contains("Ignore")));

        /// <summary>
        /// 型を定義するためのキーワード、クラスなら class、構造体なら struct … を取得する。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetDefinitionKeyword(this Type t)
        {
            var ti = t.GetTypeInfo();
            if (ti.IsEnum) return "enum";
            if (ti.IsInterface) return "interface";
            if (typeof(Delegate).GetTypeInfo().IsAssignableFrom(ti)) return "delegate";
            if (typeof(ValueType).GetTypeInfo().IsAssignableFrom(ti)) return "struct";
            if (ti.IsClass && !ti.IsValueType) return "class";
            throw new ArgumentException();
        }

        /// <summary>
        /// 整数型かどうか。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsInteger(this Type t)
            =>  t == typeof(byte)   ||  t == typeof(sbyte)  ||
                t == typeof(short)  ||  t == typeof(ushort) ||
                t == typeof(int)    ||  t == typeof(uint)   ||
                t == typeof(long)   ||  t == typeof(ulong);

        /// <summary>
        /// Nullableの整数型かどうか。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullableInteger(this Type t) => t.IsNullable() && t.ToNonNullableType().IsInteger();

        /// <summary>
        /// 数値型かどうか。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNumber(this Type t) => t.IsInteger() || t == typeof(float) || t == typeof(double) || t == typeof(decimal);

        /// <summary>
        /// Nullableの数値型かどうか。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullableNumber(this Type t) => t.IsNullable() && t.ToNonNullableType().IsNumber();

        /// <summary>
        /// C#のプリミティブ型かどうか。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsPrimitive(this Type t) => t.IsNumber() || t == typeof(string) || t == typeof(bool) || t.GetTypeInfo().IsEnum;

        /// <summary>
        /// C#のデフォルト値を表現する構文文字列を取得。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string GetDefaultValueString(this Type t, string typeName)
        {
            if (t == typeof(byte)
                || t == typeof(sbyte)
                || t == typeof(short)
                || t == typeof(ushort)
                || t == typeof(int)
                || t == typeof(uint)
                || t == typeof(long)
                || t == typeof(ulong)
                || t == typeof(float)
                || t == typeof(double)
                )
                return "0";

            if (t == typeof(bool)) return "false";

            var ti = t.GetTypeInfo();
            if (ti.IsEnum) return "0";

            if(t.IsStruct()) return "default(" + typeName + ")";
            if (t.IsGenericParameter) return "default(" + typeName + ")";

            return "null";
        }

        /// <summary>
        /// 参照型 or Nullable[T] 判定。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullable(this TypeInfo t) => !t.IsValueType || IsNullableStruct(t);

        /// <summary>
        /// null を許容するかどうか。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type t) => IsNullable(t.GetTypeInfo());

        /// <summary>
        /// <see cref="Nullable{T}"/>だったら T の <see cref="Type"/>を取得。
        /// <see cref="Nullable{T}"/>以外なら null を返す。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Type GetNonNullable(this Type t)
        {
            var ti = t.GetTypeInfo();
            if (ti.IsNullableStruct())
                return t.GenericTypeArguments[0];
            else
                return null;
        }

        /// <summary>
        /// Nullable[T] かどうか判定。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsNullableStruct(this TypeInfo t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// Nullable[T] かどうか判定。
        /// </summary>
        public static bool IsNullableStruct(this Type t) => IsNullableStruct(t.GetTypeInfo());

        /// <summary>
        /// 構造体かどうか判定。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsStruct(this TypeInfo t) => t.IsValueType && !t.IsEnum && !t.IsPrimitive;

        /// <summary>
        /// 構造体かどうか判定。
        /// </summary>
        public static bool IsStruct(this Type t) => IsStruct(t.GetTypeInfo());

        /// <summary>
        /// IEnumerable or 配列かどうか。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsCollection(this Type t)
        {
            if (t == typeof(object)) return false;
            if (t == typeof(string)) return false;

            var ti = t.GetTypeInfo();
            if (ti.IsPrimitive) return false;

            if (t.IsArray)
            {
                return true;
            }
            if (ti.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return true;
            }
            return ti.ImplementedInterfaces.Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        /// <summary>
        /// 配列の要素の<see cref="Type"/>を取得。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Type GetCollectionElementType(this TypeInfo t)
        {
            if (t.IsArray)
            {
                return t.GetElementType();
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var gt = t.GetGenericTypeDefinition();

                if (gt == typeof(Nullable<>) || gt == typeof(IEnumerable<>))
                    return t.GenericTypeArguments[0];
            }

            var ie = t.ImplementedInterfaces.FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (ie != null)
            {
                return ie.GenericTypeArguments[0];
            }

            return null;
        }

        /// <summary>
        /// 配列の要素の<see cref="Type"/>を取得。
        /// </summary>
        public static Type GetCollectionElementType(this Type t) => GetCollectionElementType(t.GetTypeInfo());


        /// <summary>
        /// nullable 化。
        /// 元々 nullable な参照型/Nullable[T] 型はそのまま返す。
        /// そうでなければ、Nullable[T] 型を作って返す。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Type ToNullableType(this Type t)
        {
            if (t.GetTypeInfo().IsClass) return t;
            if (t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) return t;
            return typeof(Nullable<>).MakeGenericType(t);
        }

        /// <summary>
        /// <see cref="Nullable{T}"/> から T を取り出す。
        /// nullable でなければ null を返す。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Type ToNonNullableType(this Type t)
        {
            var ti = t.GetTypeInfo();
            if (ti.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                return ti.GenericTypeArguments[0];
            else
                return null;
        }

        /// <summary>
        /// get も set も両方 public かどうかの判定。
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool HasPublicReadWrite(this PropertyInfo p)
        {
            if (!p.CanRead || !p.CanWrite)
                return false;

            if (p.GetMethod == null)
                return false;
            if (p.SetMethod == null)
                return false;

            return true;
        }

        /// <summary>
        /// パラメーターの型を返したいんだけども、out T の場合は T だけ返す。
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Type GetDereferencedType(this ParameterInfo p) => p.IsOut ? p.ParameterType.GetElementType() : p.ParameterType;
    }
}
