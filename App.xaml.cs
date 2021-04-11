using AirportSimulation.Model;
using AirportSimulation.ViewModel;
using System.Windows;

namespace AirportSimulation
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var model = new MainWindowModel();
            if (!model.TryLoadSchedule(out FlightsCollection.LoadResult loadResult))
            {
                MessageBox.Show($"Ошибка загрузки файла раписания рейсов: {loadResult.ErrorMessage}");
                Shutdown();
                return;
            }

            var viewModel = new MainWindowViewModel(model);
            model.Timer.Start();
            var view = new MainWindow { DataContext = viewModel };
            view.Show();
        }
    }
}
