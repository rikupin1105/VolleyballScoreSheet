using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using VolleyballScoreSheet.Model;

namespace VolleyballScoreSheet.ViewModels.Card
{
    public class YellowCardViewModel : BindableBase, IDialogAware
    {
        private readonly Game _game;
        private readonly DialogService _dialogService;
        public YellowCardViewModel(Game game, DialogService dialogService)
        {
            _game = game;
            _dialogService=dialogService;

            LeftTeamName = _game.LeftTeam.Name.Value;
            RightTeamName = _game.RightTeam.Name.Value;
            LeftTeamColor = _game.LeftTeam.Color.Value;
            RightTeamColor = _game.RightTeam.Color.Value;

            CancelCommand.Subscribe(_ => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
            LeftCommand.Subscribe(_ => YellowCard(true));
            RightCommand.Subscribe(_ => YellowCard(false));
        }
        private void YellowCard(bool isLeft)
        {
            Team team;
            Team oponentTeam;
            char AorB;
            if (isLeft == _game.isATeamLeft.Value)
            {
                team = _game.LeftTeam;
                oponentTeam = _game.RightTeam;
                AorB = 'A';
            }
            else
            {
                team = _game.RightTeam;
                oponentTeam = _game.LeftTeam;
                AorB = 'B';
            }


            //ATeam
            if (_game.Sanctions.Value
                .Where(x => x.Team==AorB)
                .Where(x => x.Warning is not "D")
                .Where(x => x.Warning is not null)
                .Any())
            {
                //二回目のイエロー
                _dialogService.ShowDialog("NotificationDialog", new DialogParameters()
                {
                     {"Title","注意" },
                     { "Message",$"すでにイエローカードが適用されています。\nセカンドレフェリーにイエローカードが2回目以降であることを通知し、\nペナルティ(赤)の適用を勧めてください。"},
                     {"ButtonText","OK" }
                }, res => { RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)); });
            }
            else
            {
                //イエロー適用 人選択へ
                _dialogService.ShowDialog("SelectPlayerAndStaff", new DialogParameters()
                {
                    { "isLeft" , isLeft }
                }, res =>
                {
                    if (res.Parameters.TryGetValue("Mark", out string mark))
                    {
                        _game.Sanctions.Value.Add(new Model.Sanction { Set = _game.Set.Value ,Warning= mark, Point=team.Sets[^1].Points.Value, OpponentPoint = oponentTeam.Sets[^1].Points.Value, Team=AorB });
                        _game.History.HistoryAdd($"YellowCard{AorB}", mark);

                        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                    }
                });
            }

        }

        public ReactiveCommand CancelCommand { get; set; } = new();
        public ReactiveCommand LeftCommand { get; set; } = new();
        public ReactiveCommand RightCommand { get; set; } = new();

        public string LeftTeamName { get; set; }
        public string RightTeamName { get; set; }
        public string LeftTeamColor { get; set; }
        public string RightTeamColor { get; set; }

        public string? Title { get; set; } = "イエローカード";
        public string? Message { get; set; }

        public event Action<IDialogResult>? RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Title", out string title))
            {
                Title = title;
            }
            if (parameters.TryGetValue("Message", out string message))
            {
                Message = message;
            }
        }
    }
}
