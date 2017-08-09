namespace SampleWpfApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new SampleData
            {
                PlainPoint = new Point(1, 2),
                Point = new Point(3, 4),
                PlainPair = new NameValuePair("abc", 123),
                Pair = new NameValuePair("xyz", 999),
                PlainUser = new User(1, "Alice", "4-2-8, ShibaKoen, Minato"),
                User = new User(2, "Bob", "1-1-2, Oshiage, Sumida"),
            };
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var data = (SampleData)DataContext;
            data.User.SetPropertyValue("Id", 100);
            data.PlainUser.Id = 100;
        }
    }
}
