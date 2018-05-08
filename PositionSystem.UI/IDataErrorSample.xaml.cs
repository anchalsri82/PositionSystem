using PositionSystem.UI.ViewModels;
using System.Windows;

namespace PositionSystem.UI
{
    /// <summary>
    /// Interaction logic for IDataErrorSample.xaml
    /// </summary>
    public partial class IDataErrorSample : Window
    {
        public IDataErrorSample()
        {
            InitializeComponent();
            DataContext = new PersonViewModel();
        }
    }
}
