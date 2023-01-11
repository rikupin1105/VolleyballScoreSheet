using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps;
using Wpf.Ui.Mvvm.Interfaces;
using VolleyballScoreSheet._3SetScoresheet.Model;
using VolleyballScoreSheet._3SetScoresheet.ViewModels;
using VolleyballScoreSheet._3SetScoresheet.Views;

namespace VolleyballScoreSheet._3SetScoresheet.Model
{
    public static class Printer
    {
        public static void Print(ScoreSheetViewModel viewModel)
        {
            var fixedDoc = new FixedDocument();
            var objReportToPrint = new ScoreSheet();

            var ReportToPrint = objReportToPrint as UserControl;
            ReportToPrint.DataContext = viewModel;

            var pageContent = new PageContent();
            var fixedPage = new FixedPage() { Width = 1122.51, Height = 793.7 };

            //Create first page of document
            fixedPage.Children.Add(ReportToPrint);
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            SendFixedDocumentToPrinter(fixedDoc);
        }
        private static void SendFixedDocumentToPrinter(FixedDocument fixedDocument)
        {
            XpsDocumentWriter xpsdw;

            PrintDocumentImageableArea imgArea = null;
            //こちらのオーバーロードだと、プリンタ選択ダイアログが出る。
            xpsdw = PrintQueue.CreateXpsDocumentWriter(ref imgArea);

            //var ps = new LocalPrintServer();
            //var pq = ps.DefaultPrintQueue; 
            //こちらのオーバーロードだと、プリンタ選択ダイアログを飛ばして既定のプリンタにスプールされる
            //xpsdw = PrintQueue.CreateXpsDocumentWriter(pq);
            try
            {
                xpsdw.Write(fixedDocument);
            }
            catch (Exception)
            {

            }
        }
    }
}
