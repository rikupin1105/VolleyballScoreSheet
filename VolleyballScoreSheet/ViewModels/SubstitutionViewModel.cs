using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Services.Dialogs;
using Reactive.Bindings;

namespace VolleyballScoreSheet.ViewModels
{
    public class SubstitutionViewModel : IDialogAware
    {
        private readonly IDialogService _dialogService;
        private readonly Game _game;
        public SubstitutionViewModel(Game game, IDialogService dialogService)
        {
            _game = game;
            _dialogService = dialogService;
            CancelCommand.Subscribe(_ => RequestClose.Invoke(new DialogResult(ButtonResult.Cancel)));
            SubstitutionCommand.Subscribe(_ => Substitution());
            SelectionChangedCommand.Subscribe(_ => SelectionChanged());
        }
        public void Substitution()
        {
            var flag = true;

            if (OutMember is not null && InMember is not null)
            {
                if (TeamName.Value == _game.ATeam.Value.Name.Value)
                {
                    //ATeam
                    var 今入った選手 = _game.ATeam.Value.Sets[^1].SubstitutionDetails
                    .Where(x => x.Point==_game.ATeam.Value.Sets[^1].Points.Value)
                    .Where(x => x.OpponentPoint==_game.BTeam.Value.Sets[^1].Points.Value).Select(x => x.In).ToArray();

                    if (今入った選手.Contains(int.Parse(OutMember)))
                    {
                        //不当な要求
                        //怪我等の場合は認める
                        _dialogService.ShowDialog("SameInterruptionSubstitution", new DialogParameters
                        {
                            {"Title","注意" },
                            { "Message",$"同一中断中での選手交代です。\n怪我などやむを得ない場合は承認を、\nそれ以外の場合はセカンドレフェリーに確認し、拒否してください。"},
                        }, res =>
                        {
                            if (res.Result == ButtonResult.OK)
                            {
                                _game.Substitution(true, int.Parse(InMember), int.Parse(OutMember));
                            }
                            else if (res.Result == ButtonResult.Abort)
                            {

                            }
                            else
                            {
                                flag = false;
                            }
                        }, "AlertWindow");
                    }
                    else
                    {
                        _game.Substitution(true, int.Parse(InMember), int.Parse(OutMember));
                    }
                }
                else
                {
                    //BTeam
                    _game.Substitution(false, int.Parse(InMember), int.Parse(OutMember));
                }

                if (flag)
                {
                    RequestClose.Invoke(new DialogResult());
                }
            }
        }
        public string OutMember { get; set; }
        public string InMember { get; set; }
        public void Dialog(string message)
        {
            _dialogService.ShowDialog(
                    "NotificationDialog",
                    new DialogParameters
                    {
                        { "Title", "Alert" },
                        { "Message", message },
                        { "ButtonText", "OK" }
                    }, res =>
                    {

                    }, "AlertWindow");
        }



        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue("Team", out string team))
            {
                if (team=="A")
                {
                    TeamName.Value  = _game.ATeam.Value.Name.Value;

                    var 選手交代で出たことある人リスト = _game.ATeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.Out).ToList();
                    var 選手交代で入ったことある人リスト = _game.ATeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.In).ToList();

                    var 選手交代で下がれる人リスト = _game.ATeam.Value.Sets[^1].Rotation.Value
                        .Except(選手交代で出たことある人リスト).OrderBy(x => x).ToArray();


                    OnCourtMemberItem.Value = 選手交代で下がれる人リスト;
                }
                else
                {
                    TeamName.Value  = _game.BTeam.Value.Name.Value;

                    var 選手交代で出たことある人リスト = _game.BTeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.Out).ToList();
                    var 選手交代で入ったことある人リスト = _game.BTeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.In).ToList();

                    var 選手交代で下がれる人リスト = _game.BTeam.Value.Sets[^1].Rotation.Value
                        .Except(選手交代で出たことある人リスト).OrderBy(x => x).ToArray();


                    OnCourtMemberItem.Value = 選手交代で下がれる人リスト;
                }
            }
        }
        public void SelectionChanged()
        {
            if (TeamName.Value == _game.ATeam.Value.Name.Value)
            {
                var 選手交代で出たことある人リスト = _game.ATeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.Out).ToList();
                var 選手交代で入ったことある人リスト = _game.ATeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.In).ToList();

                var 選手交代で入れる人リスト = _game.ATeam.Value.Players
                    .Select(x => x.Id)
                    .Except(_game.ATeam.Value.Sets[^1].Rotation.Value)
                    .Except(選手交代で入ったことある人リスト)
                    .OrderBy(x => x)
                    .ToArray();


                if (選手交代で入ったことある人リスト.Contains(int.Parse(OutMember)))
                {
                    //再入場
                    OutCourtMemberItem.Value = _game.ATeam.Value.Sets[^1].SubstitutionDetails
                        .Where(x => x.In==int.Parse(OutMember))
                        .Select(x => x.Out)
                        .ToArray();
                }
                else
                {
                    OutCourtMemberItem.Value = 選手交代で入れる人リスト.Except(選手交代で出たことある人リスト).ToArray();
                }
            }
            else
            {
                var 選手交代で出たことある人リスト = _game.BTeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.Out).ToList();
                var 選手交代で入ったことある人リスト = _game.BTeam.Value.Sets[^1].SubstitutionDetails.Select(x => x.In).ToList();

                var 選手交代で入れる人リスト = _game.BTeam.Value.Players
                    .Select(x => x.Id)
                    .Except(_game.BTeam.Value.Sets[^1].Rotation.Value)
                    .Except(選手交代で入ったことある人リスト)
                    .OrderBy(x => x)
                    .ToArray();


                if (選手交代で入ったことある人リスト.Contains(int.Parse(OutMember)))
                {
                    //再入場
                    OutCourtMemberItem.Value = _game.BTeam.Value.Sets[^1].SubstitutionDetails
                        .Where(x => x.In==int.Parse(OutMember))
                        .Select(x => x.Out)
                        .ToArray();
                }
                else
                {
                    OutCourtMemberItem.Value = 選手交代で入れる人リスト.Except(選手交代で出たことある人リスト).ToArray();
                }
            }
        }
        public ReactiveCommand SelectionChangedCommand { get; } = new();
        public ReactiveCommand CancelCommand { get; } = new();
        public ReactiveCommand SubstitutionCommand { get; } = new();
        public ReactiveProperty<string> TeamName { get; set; } = new();
        public ReactiveProperty<int[]> OnCourtMemberItem { get; set; } = new();
        public ReactiveProperty<int[]> OutCourtMemberItem { get; set; } = new();
        public string Title => "メンバーチェンジ";
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
    }
}
