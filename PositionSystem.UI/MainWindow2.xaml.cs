using System.Windows;
using System.Windows.Input;

namespace PositionSystem.UI
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
        }

        //Bubbling
        private void btnClickMe_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("I am outer button");
        }

        private void InnerButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("I am inner button");
        }

        private void OuterEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("I'm green eclipse");
            // In this case button click will be not routed 
        }

        private void InnerEllipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Title = "Yellow Eclipse has fired";
            // In this case button click will be routed 
        }

        //Tunneling
        private void btnClickMe_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Move by outer button");
        }

        private void InnerButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Move by inner button");
        }
    }
}
