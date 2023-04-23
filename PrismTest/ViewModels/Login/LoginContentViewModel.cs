using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace PrismTest.ViewModels.Login
{
    public class LoginContentViewModel:BindableBase
    {

        private readonly IContainerExtension _container;
        private readonly IRegionManager _regionManager;



        public LoginContentViewModel(IContainerExtension container, IRegionManager regionManager)
        {
            _container=container;
            _regionManager=regionManager;
        }
    }
}
