using ImportEcritureComptable.Model;
using Microsoft.Win32;
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

namespace ImportEcritureComptable.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void FindFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Fichiers Excel (*.xlsx)|*.xlsx",
                CheckFileExists = true,
                Multiselect = false,
            };
            if (dialog.ShowDialog() ?? false && !string.IsNullOrEmpty(dialog.FileName))
            {
                TextBoxFileName.Text = dialog.FileName;


                MainWindow.EcrituresComptable = EcritureComptable.ReadExcelFile(dialog.FileName);
                ActivateButtonImport();
                //if (ComboBoxCodeJournal.SelectedItem != null)
                //{
                //    ActivateButtonImport();
                //}

            }
        }

        private void ActivateButtonImport()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.ImportBtn.IsEnabled = true;
            }
        }
    }
}
