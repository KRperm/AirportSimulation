using System.Windows;
using System.Windows.Controls;

namespace AirportSimulation
{
    /// <summary>
    /// Логика взаимодействия для PassengerTable.xaml
    /// </summary>
    public partial class PassengerTable : UserControl
    {
        public static DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(PassengerTable));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public PassengerTable()
        {
            InitializeComponent();
        }
    }
}
