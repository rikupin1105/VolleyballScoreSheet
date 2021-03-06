using System.Windows;
using VolleyballScoreSheet.ViewModels;
using VolleyballScoreSheet.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using VolleyballScoreSheet;
using Prism.Modularity;

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
            containerRegistry.RegisterForNavigation<MatchInfo>();
            containerRegistry.RegisterForNavigation<RosterA>();
            containerRegistry.RegisterForNavigation<RosterB>();
            containerRegistry.RegisterForNavigation<CoinToss>();
            containerRegistry.RegisterForNavigation<BeforeMatch>();
            containerRegistry.RegisterForNavigation<Gaming>();

            containerRegistry.RegisterDialog<Rotation>();
            containerRegistry.RegisterDialog<NotificationDialog>();
            containerRegistry.RegisterDialog<Substitution>();
            containerRegistry.RegisterDialog<ExceptionalSubstitution>();

            containerRegistry.RegisterDialogWindow<DialogWindow>("DialogWindow");
            containerRegistry.RegisterDialogWindow<AlertWindow>("AlertWindow");

            containerRegistry.RegisterSingleton<Game>();
        }
    }
}
