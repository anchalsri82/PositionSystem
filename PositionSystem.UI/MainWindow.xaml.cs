using System;
using System.ComponentModel;
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
    public class Person : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateTimeAdded { get; set; }

        #region IDataErrorInfo members
        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;
                switch (propertyName)
                {
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            result = "Name is required!";
                        break;
                    case "Address":
                        if (string.IsNullOrEmpty(Address))
                            result = "Address is required";
                        break;

                }
                return result;
            }
        }
        public string Error => throw new NotImplementedException();
        #endregion

        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

    public class Manager : Person
    {

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
