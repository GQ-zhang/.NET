using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using PrismTest.Views.Login;
using UtilityLibrary;

namespace PrismTest.ViewModels.Login
{ 
    public class LoginWindowViewModel:BindableBase
    {
        private readonly IContainerExtension _container;

        private readonly IRegionManager _regionManager;

        public LoginWindowViewModel(IContainerExtension container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }


        private DelegateCommand _loadCommand;

        public DelegateCommand LoginLoadingCommand => _loadCommand ??= new DelegateCommand(ExcuteLoadCommand);


        private void ExcuteLoadCommand()
        {
            _regionManager.Regions[RegionNames.LoginRegion].RequestNavigate(nameof(LoginContentView));
        }
    }
}
