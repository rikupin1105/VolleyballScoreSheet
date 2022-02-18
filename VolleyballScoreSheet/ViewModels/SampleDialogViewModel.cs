using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyballScoreSheet.ViewModels
{
    internal class SampleDialogViewModel : BindableBase, IDialogAware
    {
        public string _message = "";
        public DelegateCommand CloseDialogCommand { get; private set; }

        public string Title => "サンプルダイアログ";

        public event Action<IDialogResult> RequestClose;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SampleDialogViewModel()
        {
            CloseDialogCommand = new DelegateCommand(CloseDialog);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            _message = parameters?.GetValue<string>("msg");
        }

        private void CloseDialog()
        {
            IDialogParameters param = new DialogParameters
            {
                { "result", DateTime.Now.ToLongTimeString() }
            };

            CloseDialog(new DialogResult(ButtonResult.OK, param));
        }

        /// <summary>
        /// ダイアログを閉じ遷移元の画面に戻る
        /// </summary>
        protected void CloseDialog(IDialogResult result)
        {
            RequestClose.Invoke(result);
        }

        /// <summary>
        /// ダイアログクローズ前に呼ばれる
        /// （ダイアログを閉じる前に入力の検証や、ユーザへの確認を行うことが出来る）
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// ダイアログクローズ後に呼ばれる
        /// </summary>
        public void OnDialogClosed()
        {
        }
    }
}
