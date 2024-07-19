using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportEcritureComptable.Model
{
    public class EcritureComptable
    {
        public string CodeJournal { get; set; }
        public string LibJournal { get; set; }

        public string EcritureDate { get; set; }
        public string EcritureLib { get; set;}

        public string CompteNum { get; set; }
        public string CompteLib {  get; set; }

        public string PieceRef { get; set; }
        public string PieceDate { get; set; }

        public double Debit { get; set; }
        public double Credit { get; set; }

        public string Client { get; set; }
        public string ClientCode { get; set; }

        public string PaiementMode { get; set; }
        public double QteProduit { get; set; }
        public double ProduitPrixUnitaireHt { get; set;}


        public string PaysTaxation { get; set; }

        EcritureComptable() { }

        public static List<EcritureComptable> ReadExcelFile(string filePath)
        {
            var list = new List<EcritureComptable>();

            // Configuration pour autoriser les fichiers non signés (si nécessaire)
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.First();
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++) // Assume that row 1 has column headers
                {
                    var model = new EcritureComptable
                    {
                        CodeJournal = worksheet.Cells[row, 1].Text,
                        LibJournal = worksheet.Cells[row, 2].Text,
                        EcritureDate = worksheet.Cells[row, 3].Text,
                        EcritureLib = worksheet.Cells[row, 4].Text,
                        CompteNum = worksheet.Cells[row, 5].Text,
                        CompteLib = worksheet.Cells[row, 6].Text,
                        PieceRef = worksheet.Cells[row, 7].Text,
                        PieceDate = worksheet.Cells[row, 8].Text,
                        Debit = double.TryParse(worksheet.Cells[row, 9].Text, out double debit) ? debit : 0,
                        Credit = double.TryParse(worksheet.Cells[row, 10].Text, out double credit) ? credit : 0,
                        Client = worksheet.Cells[row, 11].Text,
                        ClientCode = worksheet.Cells[row, 12].Text,
                        PaiementMode = worksheet.Cells[row, 13].Text,
                        QteProduit = double.TryParse(worksheet.Cells[row, 14].Text, out double qteProduit) ? qteProduit : 0,
                        ProduitPrixUnitaireHt = double.TryParse(worksheet.Cells[row, 15].Text, out double produitPrixUnitaireHt) ? produitPrixUnitaireHt : 0,
                        PaysTaxation = worksheet.Cells[row, 16].Text
                    };

                    list.Add(model);
                }
            }

            return list;
        }

    }
}
