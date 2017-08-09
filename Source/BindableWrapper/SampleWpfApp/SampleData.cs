using BindableHelper;

namespace SampleWpfApp
{
    /// <summary>
    /// フィールドむき出しでプレーンな構造体。
    /// </summary>
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    /// <summary>
    /// プレーンな構造体。
    /// </summary>
    public struct NameValuePair
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public NameValuePair(string name, int value) : this()
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// プレーンなクラス。
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public User(int id, string name, string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }
    }

    /// <summary>
    /// 上記3つの型を、それぞれ生で持つのと、<see cref="BindableWrapper{T}"/>越しに持つ型。
    /// </summary>
    public class SampleData
    {
        public Point PlainPoint { get; set; }
        public BindableWrapper<Point> Point { get; set; }

        public NameValuePair PlainPair { get; set; }
        public BindableWrapper<NameValuePair> Pair { get; set; }

        public User PlainUser { get; set; }
        public BindableWrapper<User> User { get; set; }
    }
}
