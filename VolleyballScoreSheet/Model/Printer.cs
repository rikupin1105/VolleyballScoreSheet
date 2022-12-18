﻿using System;
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
using VolleyballScoreSheet.ViewModels;
using VolleyballScoreSheet.ViewModels.ScoreSheet;
using VolleyballScoreSheet.Views.ScoreSheet;
using Wpf.Ui.Mvvm.Interfaces;

namespace VolleyballScoreSheet.Model
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
            //こちらのオーバーロードだと、プリンタ選択ダイアログが出る。
            LocalPrintServer lps = new LocalPrintServer();
            PrintQueue queue = lps.DefaultPrintQueue;
            XpsDocumentWriter xpsdw = PrintQueue.CreateXpsDocumentWriter(queue);

            PrintTicket ticket = queue.DefaultPrintTicket;
            ticket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA3);
            ticket.PageOrientation = PageOrientation.Landscape;
            //var ps = new LocalPrintServer();
            //var pq = ps.DefaultPrintQueue; 
            //こちらのオーバーロードだと、プリンタ選択ダイアログを飛ばして既定のプリンタにスプールされる
            //xpsdw = PrintQueue.CreateXpsDocumentWriter(pq);
            xpsdw.Write(fixedDocument,ticket);
        }
    }
}