using System.Windows;
using BasicRegionNavigation.ViewModels;
using BasicRegionNavigation.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using VolleyballScoreSheet;

namespace BasicRegionNavigation
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
            containerRegistry.RegisterDialogWindow<DialogWindow>("DialogWindow");
            //containerRegistry.RegisterForNavigation<Rotation>();

            //containerRegistry.RegisterDialogWindow<Rotation>();
            //containerRegistry.RegisterDialog<Rotation>();

            //containerRegistry.RegisterSingleton<Prism.Services.Dialogs.IDialogService, StyleableDialogService>();
            containerRegistry.RegisterSingleton<Game>();
        }
    }
}
