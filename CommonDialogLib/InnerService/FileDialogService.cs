﻿using System.Linq;
using Microsoft.Win32;

namespace CommonDialogLib.InnerService
{
    class FileDialogService : ICommonDialogService
    {
        /// <summary>コモンダイアログを表示します。</summary>
        /// <param name="settings">設定情報を表すIDialogSettings。</param>
        /// <returns>trueが返ると選択したファイル名、ユーザがキャンセルするとfalseが返ります。</returns>
        public bool ShowDialog(ICommonDialogSettings settings)
        {
            var dialog = this.CreateDialogService(settings);
            if (dialog == null)
                return false;

            var ret = dialog.ShowDialog();
            if (ret.HasValue)
            {
                this.setReturnValues(dialog, settings);
                return ret.Value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>表示するコモンダイアログを生成します。</summary>
        /// <param name="settings">設定情報を表すIDialogSettings。</param>
        /// <returns>生成したコモンダイアログを表すFileDialog。</returns>
        private FileDialog? CreateDialogService(ICommonDialogSettings settings)
        {
            if (settings == null)
                return null;

            FileDialog? dialog = null;

            if (settings is OpenFileDialogSettings)
                dialog = new OpenFileDialog();
            else if (settings is SaveFileDialogSettings)
                dialog = new SaveFileDialog();
            else
                return null;

            var saveSettings = settings as SaveFileDialogSettings;

            dialog.Filter = saveSettings.Filter;
            dialog.FilterIndex = saveSettings.FilterIndex;
            dialog.InitialDirectory = saveSettings.InitialDirectory;
            dialog.Title = saveSettings.Title;

            return dialog;
        }

        /// <summary>戻り値を設定します。</summary>
        /// <param name="dialog">表示したダイアログを表すFileDialog。</param>
        /// <param name="settings">設定情報を表すIDialogSettings。</param>
        private void setReturnValues(FileDialog dialog, ICommonDialogSettings settings)
        {
            switch (settings)
            {
                case OpenFileDialogSettings o:
                    var openDialog = dialog as OpenFileDialog;
                    o.FileName = openDialog!.FileName;
                    o.FileNames = openDialog.FileNames.ToList();
                    break;
                case SaveFileDialogSettings s:
                    var saveDialog = dialog as SaveFileDialog;
                    s.FileName = saveDialog!.FileName;
                    break;
                default:
                    break;
            }
        }
    }
}