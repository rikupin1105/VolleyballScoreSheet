using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VolleyballScoreSheet.ViewModels
{
    public class StartViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public StartViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NewGameCommand.Subscribe(_ => Navigate("MatchInfo"));
        }
        public ReactiveCommand NewGameCommand { get; } = new();

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
