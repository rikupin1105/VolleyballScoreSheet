using System.Windows;
using VolleyballScoreSheet.ViewModels;
using VolleyballScoreSheet.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using VolleyballScoreSheet;
using Prism.Modularity;
using Wpf.Ui.Mvvm.Contracts;
using VolleyballScoreSheet.Views.Card;
using VolleyballScoreSheet.Views.Dialog;
using CommonDialogLib;

namespace VolleyballScoreSheet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow>();

            containerRegistry.RegisterForNavigation<Views.MatchInfo>();
            containerRegistry.RegisterForNavigation<Roster>();
            containerRegistry.RegisterForNavigation<Views.CoinToss>();
            containerRegistry.RegisterForNavigation<BeforeMatch>();
            containerRegistry.RegisterForNavigation<Gaming>();

            containerRegistry.RegisterDialog<Rotation>();
            containerRegistry.RegisterDialog<NotificationDialog>();
            containerRegistry.RegisterDialog<Views.Substitution>();
            containerRegistry.RegisterDialog<ExceptionalSubstitution>();
            containerRegistry.RegisterDialog<SameInterruptionSubstitution>();
            containerRegistry.RegisterDialog<Card>();
            containerRegistry.RegisterDialog<Views.Card.SelectTeam>();
            containerRegistry.RegisterDialog<ConfirmDialog>();
            containerRegistry.RegisterDialog<SelectPlayerAndStaff>();
            containerRegistry.RegisterDialog<Views.Referees>();
            containerRegistry.RegisterDialog<EditPlayer>();

            containerRegistry.RegisterDialogWindow<DialogWindow>("DialogWindow");
            containerRegistry.RegisterDialogWindow<AlertWindow>("AlertWindow");

            containerRegistry.RegisterSingleton<ICommonDialogService, CommonDialogService>();
            containerRegistry.RegisterSingleton<Game>();
        }
    }
}
