using System.ComponentModel;

// 名前空間は(仮)
namespace BindableHelper
{
    /// <summary>
    /// plain なクラス・構造体に対して、PropertyChanged 通知付きでデータバインディングするためのクラス。
    /// </summary>
    /// <remarks>
    /// <see cref="SetPropertyValue(string, object)"/> を呼ぶと、<see cref="Value"/>のプロパティが書き換わったうえで、<see cref="PropertyChanged"/>が飛ぶ。
    /// <see cref="System.Reflection.ICustomTypeProvider"/>で型偽装していて、
    /// { Binding X } とかバインディングすると、SetPropertyValue("X", value) が呼ばれて、結果的に Value.X が書き換わる。
    /// </remarks>
    /// <typeparam name="T">元となる型。</typeparam>
    public partial class BindableWrapper<T> : INotifyPropertyChanged
    {
        /// <summary>
        /// 元となるインスタンス。
        /// </summary>
        public T Value;

        static BindableWrapper()
        {
            InitializeAccessors();
        }

        /// <summary>
        /// default 値で初期化。
        /// </summary>
        public BindableWrapper() { }

        /// <summary>
        /// 値を渡して初期化。
        /// </summary>
        /// <param name="value"></param>
        public BindableWrapper(T value) => Value = value;

        /// <summary>
        /// 元となる型からは暗黙的変換可能。
        /// </summary>
        public static implicit operator BindableWrapper<T>(T value) => new BindableWrapper<T>(value);

        /// <summary>
        /// <see cref="Value"/>に丸投げ。
        /// </summary>
        public override string ToString() => Value.ToString();

        /// <summary>
        /// プロパティ名を指定して、そのプロパティの値を get する。
        /// </summary>
        public object GetPropertyValue(string name) => _accessors[name].Get(ref Value);

        /// <summary>
        /// プロパティ名を指定して、そのプロパティに値を set する。
        /// </summary>
        public void SetPropertyValue(string name, object value)
        {
            var (get, set, arg) = _accessors[name];

            if (!Equals(get(ref Value), value))
            {
                set(ref Value, value);
                PropertyChanged?.Invoke(this, arg);
            }
        }

        /// <summary>
        /// <see cref="INotifyPropertyChanged"/>
        ///
        /// <see cref="SetPropertyValue(string, object)"/>で、値が変化していたら呼ばれる。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
