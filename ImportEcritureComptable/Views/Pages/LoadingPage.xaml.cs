using ImportEcritureComptable.Core;
using ImportEcritureComptable.Model;
using Objets100cLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Logique d'interaction pour LoadingPage.xaml
    /// </summary>
    public partial class LoadingPage : Page
    {
        public event Action ProcessingCompleted;
        private IEnumerable<Model.EcritureComptable> EcrituresComptable;
        public LoadingPage()
        {
            InitializeComponent();
        }

        public LoadingPage(List<Model.EcritureComptable> ecritureComptables)
        {
            InitializeComponent();
            EcrituresComptable = ecritureComptables;

            StartProcessing();
        }

        private async void StartProcessing()
        {
            BSCIALApplication100c sageApplication = SageAuths.OpenOM();
            if (sageApplication!= null)
            {
                await Task.Run(() =>
                {
                    int MaxCount = EcrituresComptable.Count();
                    int ActualCount = 0;
                    Dispatcher.Invoke(() =>
                    {
                        ProgressContent.Content = $"0/{MaxCount}";
                        LoadingProgress.Maximum = MaxCount;
                        LoadingProgress.Value = ActualCount;
                    });


                    foreach (Model.EcritureComptable ecritureComptable in EcrituresComptable)
                    {
                        ActualCount++;

                        Dispatcher.Invoke(() =>
                        {
                            ProgressContent.Content = $"{ActualCount}/{MaxCount}";
                            LoadingProgress.Value = ActualCount;
                        });


                        IBOEcriture3 ecriture = (IBOEcriture3)sageApplication.CptaApplication.FactoryEcriture.Create();
                        ecriture.Journal = sageApplication.CptaApplication.FactoryJournal.ReadNumero(ecritureComptable.CodeJournal);
                        DateTime date = DateTime.Parse(ecritureComptable.EcritureDate);
                        ecriture.Date = date;

                        ecriture.EC_Intitule = Truncate(ecritureComptable.EcritureLib, 35);

                        if (!string.IsNullOrEmpty(ecritureComptable.CompteNum))
                        {
                            ecriture.CompteG = sageApplication.CptaApplication.FactoryCompteG.ReadNumero(ecritureComptable.CompteNum);
                        }
                        if (!string.IsNullOrEmpty(ecritureComptable.ClientCode))
                        {
                            ecriture.Tiers = sageApplication.CptaApplication.FactoryTiers.ReadNumero(ecritureComptable.ClientCode);
                        }
                        if (ecritureComptable.Debit > 0)
                        {
                            ecriture.EC_Sens = EcritureSensType.EcritureSensTypeDebit;
                            ecriture.EC_Montant = ecritureComptable.Debit;
                        }
                        else if (ecritureComptable.Credit > 0)
                        {
                            ecriture.EC_Sens = EcritureSensType.EcritureSensTypeCredit;
                            ecriture.EC_Montant = ecritureComptable.Credit;
                        }
                        if (!string.IsNullOrEmpty(ecritureComptable.PaysTaxation))
                        {
                            ecriture.Taxe = sageApplication.CptaApplication.FactoryTaxe.ReadCode(ecritureComptable.PaysTaxation);
                        }
                        ecriture.EC_RefPiece = ecritureComptable.PieceRef;
                        ecriture.Reglement = sageApplication.CptaApplication.FactoryReglement.ReadIntitule(ecritureComptable.PaiementMode);
                        
                        //ecriture.EC_Piece = sageApplication.CptaApplication.FactoryEcriture.QueryJournalPeriode(ecriture.Journal,
                        //                                                                                        new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month))
                        //                                                                                        ).ToString();
                        ecriture.WriteDefault();


                    }
                });
            }

            ProcessingCompleted?.Invoke();
        }

        private static string Truncate(string field, int length)
        {
            return field.Length > length ? field.Substring(0, length) : field;
        }
    }
}
