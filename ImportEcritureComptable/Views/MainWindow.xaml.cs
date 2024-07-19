using ImportEcritureComptable.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImportEcritureComptable.Views
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Model.EcritureComptable> EcrituresComptable;
        public MainWindow()
        {
            InitializeComponent();
            frameMain.Navigate(new Views.Pages.MainPage());
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            ImportBtn.IsEnabled = false;
            var loadingPage = new Views.Pages.LoadingPage(EcrituresComptable);
          
            // Abonnez-vous à l'événement ProcessingCompleted
            loadingPage.ProcessingCompleted += OnLoadingPageProcessingCompleted;

            // Naviguer vers la page de chargement
            frameMain.Navigate(loadingPage);
        }

        private void OnLoadingPageProcessingCompleted()
        {
            // Lorsque le traitement est terminé, naviguez de retour vers la MainPage
            Dispatcher.Invoke(() =>
            {
                frameMain.Navigate(new Views.Pages.MainPage());
                ImportBtn.IsEnabled = true;
            });
        }
    }
}
