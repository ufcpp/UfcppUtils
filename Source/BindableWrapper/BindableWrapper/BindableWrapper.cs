using System.ComponentModel;

// 名前空間は(仮)
namespace BindableHelper
{
    public partial class BindableWrapper<T> : INotifyPropertyChanged
    {
        public T Value;

        static BindableWrapper()
        {
            InitializeAccessors();
        }

        public BindableWrapper() { }
        public BindableWrapper(T value) => Value = value;

        public static implicit operator BindableWrapper<T>(T value) => new BindableWrapper<T>(value);

        public override string ToString() => Value.ToString();

        public object GetPropertyValue(string name) => _accessors[name].Get(ref Value);

        public void SetPropertyValue(string name, object value)
        {
            var (get, set, arg) = _accessors[name];

            if (!Equals(get(ref Value), value))
            {
                set(ref Value, value);
                PropertyChanged?.Invoke(this, arg);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
