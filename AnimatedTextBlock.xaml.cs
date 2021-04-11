using System.Windows;
using System.Windows.Controls;

namespace AirportSimulation
{
    /// <summary>
    /// Логика взаимодействия для AnimatedTextBox.xaml
    /// </summary>
    public partial class AnimatedTextBlock : UserControl
    {
        
        public static DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(AnimatedTextBlock));

        public static DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(nameof(TextAlignment), typeof(TextAlignment), typeof(AnimatedTextBlock));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public AnimatedTextBlock()
        {
            InitializeComponent();
        }
    }
}
