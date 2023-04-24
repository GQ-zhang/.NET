using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BLL.IService;
using Dao.Model.Dto;
using HandyControl.Controls;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using PrismTest.Views;
using PrismTest.Views.Login;
using UtilityLibrary.Extend;
using Window = HandyControl.Controls.Window;

namespace PrismTest.ViewModels.Login
{
    public class LoginContentViewModel:BindableBase
    {

        private readonly IContainerExtension _container;
        private readonly IRegionManager _regionManager;

        private LoginContentView _loginContentView;


        public LoginContentViewModel(IContainerExtension container, IRegionManager regionManager)
        {
            _container=container;
            _regionManager=regionManager;
        }
       
        
        private ObservableCollection<SystemUserDto> _users=new ObservableCollection<SystemUserDto>();

        public ObservableCollection<SystemUserDto> Users
        {
            get=>_users;
            set => SetProperty(ref _users, value);
        }

        private string _userAccount;

        public string UserAccount
        {
            get => _userAccount;
            set => SetProperty(ref _userAccount, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private DelegateCommand<object> _loadCommand;
        public DelegateCommand<object> LoadedCommand => _loadCommand ?? new DelegateCommand<object>(ExcuteLoadCommand);


        private  void ExcuteLoadCommand(object obj)
        {
            _loginContentView=obj as LoginContentView;
             GetUser();
        }

        private DelegateCommand<PasswordBox> _loginCommand;

        public DelegateCommand<PasswordBox> LoginCommand =>
            _loginCommand ??= new DelegateCommand<PasswordBox>(ExcuteLoginCommand);


        private async void ExcuteLoginCommand(PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(UserAccount))
            {
                HandyControl.Controls.MessageBox.Show($"请选择用户名", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                HandyControl.Controls.MessageBox.Show($"请输入密码", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (passwordBox.Password.Length<6)
            {
                HandyControl.Controls.MessageBox.Show($"密码长度小于6，请检查密码", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var result = await _container.Resolve<ISystemUser>().SearchUserInfo();
            var userInfo = result.FirstOrDefault(it =>
                it.UserAccount == UserAccount && Encryption.Decrypt(it.UserPassword) == passwordBox.Password);
            if (userInfo != null)
            {
                Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()?.Hide();
          
                var shell = Application.Current.MainWindow = new MainWindow();
                shell?.Show();
                Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault()?.Close();
            }
            else
            {
                HandyControl.Controls.MessageBox.Show($"登录失败,密码错误", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private DelegateCommand _windowDragMoveCommand;

        public DelegateCommand WindowDragMoveCommand => new DelegateCommand(() =>
        {
            Window.GetWindow(_loginContentView)?.DragMove();
        });
        private async void GetUser()
        {
            var result=await _container.Resolve<ISystemUser>().SearchUserInfo();
            Users.Clear();
            Users.AddRange(result);
        }
    }
}
