using PositionSystem.UI.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PositionSystem.UI
{
    class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            date = DateTime.Today;

            return date.ToString("dd/MMM/yyyy");

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Counter
        {
            get { return (int)GetValue(CounterProperty); }
            set { SetValue(CounterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Counter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CounterProperty =
            DependencyProperty.Register("Counter", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public Person Person { get; set; }
        public Manager Manager { get; set; }

        public MainWindow()
        {
            
            Person = new Person
            {
                Name = "Ramesh Kumar",
                Address = "Flat E Block 8, Floor 8, Tierra Verde, Tsing Yi, HK"
            };
            Manager = new Manager
            {
                Name = "Ramesh Kumar Manager",
                Address = "Flat E Block 8, Floor 8, Tierra Verde, Tsing Yi, HK"
            };
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Resources["DynamicColor"] = new SolidColorBrush(Colors.Blue);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Resources["DynamicColor"] = new SolidColorBrush(Colors.Red);
        }
    }
}
