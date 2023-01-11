using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;

namespace VolleyballScoreSheet.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public ReactiveCommand LoadedCommand { get; set; } = new ReactiveCommand();
        public DelegateCommand<string> NavigateCommand { get; private set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            //LoadedCommand.Subscribe(() => _regionManager.RequestNavigate("ContentRegion", "MatchInfo"));
            LoadedCommand.Subscribe(() => _regionManager.RequestNavigate("ContentRegion", "Start"));
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
