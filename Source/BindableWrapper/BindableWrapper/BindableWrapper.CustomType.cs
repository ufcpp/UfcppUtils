using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace BindableHelper
{
    public partial class BindableWrapper<T> : ICustomTypeProvider
    {
        Type ICustomTypeProvider.GetCustomType() => _wrapType;

        static readonly Type _wrapType = new WrapType();

        private class WrapType : Type
        {
            PropertyInfo[] _properties;

            public WrapType()
            {
                var properties = typeof(T).GetProperties();
                var fields = typeof(T).GetFields();
                _properties = new PropertyInfo[properties.Length + fields.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    var m = properties[i];
                    _properties[i] = new WrapProperty(m.PropertyType, m);
                }

                for (int i = 0; i < fields.Length; i++)
                {
                    var m = fields[i];
                    _properties[i + properties.Length] = new WrapProperty(m.FieldType, m);
                }
            }

            public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => _properties;
            protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers) => _properties.First(x => x.Name == name);

            #region Dummy Impl
            public override RuntimeTypeHandle TypeHandle => typeof(T).TypeHandle;
            public override Type BaseType => typeof(T).BaseType;
            public override Type UnderlyingSystemType => typeof(T).UnderlyingSystemType;
            #endregion
            #region Not Impl
            public override Guid GUID => throw new NotImplementedException();
            public override Module Module => throw new NotImplementedException();
            public override Assembly Assembly => throw new NotImplementedException();
            public override string FullName => throw new NotImplementedException();
            public override string Namespace => throw new NotImplementedException();
            public override string AssemblyQualifiedName => throw new NotImplementedException();
            public override string Name => throw new NotImplementedException();
            public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => throw new NotImplementedException();
            public override object[] GetCustomAttributes(bool inherit) => throw new NotImplementedException();
            public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotImplementedException();
            public override Type GetElementType() => throw new NotImplementedException();
            public override EventInfo GetEvent(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
            public override EventInfo[] GetEvents(BindingFlags bindingAttr) => throw new NotImplementedException();
            public override FieldInfo GetField(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
            public override FieldInfo[] GetFields(BindingFlags bindingAttr) => throw new NotImplementedException();
            public override Type GetInterface(string name, bool ignoreCase) => throw new NotImplementedException();
            public override Type[] GetInterfaces() => throw new NotImplementedException();
            public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => throw new NotImplementedException();
            public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => throw new NotImplementedException();
            public override Type GetNestedType(string name, BindingFlags bindingAttr) => throw new NotImplementedException();
            public override Type[] GetNestedTypes(BindingFlags bindingAttr) => throw new NotImplementedException();
            public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters) => throw new NotImplementedException();
            public override bool IsDefined(Type attributeType, bool inherit) => throw new NotImplementedException();
            protected override TypeAttributes GetAttributeFlagsImpl() => throw new NotImplementedException();
            protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => throw new NotImplementedException();
            protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => throw new NotImplementedException();
            protected override bool HasElementTypeImpl() => throw new NotImplementedException();
            protected override bool IsArrayImpl() => throw new NotImplementedException();
            protected override bool IsByRefImpl() => throw new NotImplementedException();
            protected override bool IsCOMObjectImpl() => throw new NotImplementedException();
            protected override bool IsPointerImpl() => throw new NotImplementedException();
            protected override bool IsPrimitiveImpl() => throw new NotImplementedException();
            #endregion
        }

        private class WrapProperty : PropertyInfo
        {
            Type _memberType;
            MemberInfo _info;

            public WrapProperty(Type memberType, MemberInfo info)
            {
                _memberType = memberType;
                _info = info;
            }

            public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
                var w = (BindableWrapper<T>)obj;
                return w.GetPropertyValue(Name);
            }

            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
            {
                var w = (BindableWrapper<T>)obj;
                w.SetPropertyValue(Name, value);
            }

            #region Dummy Impl
            public override Type PropertyType => _memberType;
            public override PropertyAttributes Attributes => PropertyAttributes.None;
            public override bool CanRead => true;
            public override bool CanWrite => true;
            public override string Name => _info.Name;
            public override ParameterInfo[] GetIndexParameters() => Array.Empty<ParameterInfo>();
            public override MethodInfo GetGetMethod(bool nonPublic) => new DummyMethodInfo();
            public override MethodInfo GetSetMethod(bool nonPublic) => new DummyMethodInfo();
            class DummyMethodInfo : MethodInfo
            {
                public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();
                public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();
                public override MethodAttributes Attributes => MethodAttributes.Public;
                public override string Name => throw new NotImplementedException();
                public override Type DeclaringType => throw new NotImplementedException();
                public override Type ReflectedType => throw new NotImplementedException();
                public override MethodInfo GetBaseDefinition() => throw new NotImplementedException();
                public override object[] GetCustomAttributes(bool inherit) => throw new NotImplementedException();
                public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotImplementedException();
                public override MethodImplAttributes GetMethodImplementationFlags() => throw new NotImplementedException();
                public override ParameterInfo[] GetParameters() => throw new NotImplementedException();
                public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => throw new NotImplementedException();
                public override bool IsDefined(Type attributeType, bool inherit) => throw new NotImplementedException();
            }
            #endregion
            #region Not Impl
            public override bool IsDefined(Type attributeType, bool inherit) => throw new NotImplementedException();
            public override Type DeclaringType => throw new NotImplementedException();
            public override Type ReflectedType => throw new NotImplementedException();
            public override MethodInfo[] GetAccessors(bool nonPublic) => throw new NotImplementedException();
            public override object[] GetCustomAttributes(bool inherit) => throw new NotImplementedException();
            public override object[] GetCustomAttributes(Type attributeType, bool inherit) => throw new NotImplementedException();
            #endregion
        }
    }
}
