using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BLL.IService;
using Dao.Model.Dto;
using Prism.Ioc;
using SharpVectors.Converters;
using UtilityLibrary.Extend;

namespace PrismTest.Views
{
    /// <summary>
    /// AddUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private readonly IContainerExtension _container;
        public AddUserWindow(IContainerExtension container)
        {
            _container = container;
            InitializeComponent();
            //svgViewbox.Source = new Uri("pack://application:,,,/Client.Controls;component/Images/关闭.svg");

        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           DragMove();     
        }

        private void WindowClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void SaveUser_Click(object sender, RoutedEventArgs e)
        {
            var user = new SystemUserDto()
            {
                Id = Guid.NewGuid().ToString("N"),
                UserName = UserNameTextBox.Text,
                UserAccount = UserAccountTextBox.Text,
                UserPassword = UserPasswordBox.Password,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UserRole = 1
            };

            if (user.UserName.Length < 2)
            {
                HandyControl.Controls.MessageBox.Show("用户姓名至少有2个字符", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!Regex.IsMatch(user.UserAccount, @"^1[3456789]\d{9}$"))
            {
                HandyControl.Controls.MessageBox.Show("无效的手机号码", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (user.UserPassword.Length < 6)
            {
                HandyControl.Controls.MessageBox.Show("用户密码至少为6位", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //if (Regex.IsMatch(user.Password, @"^[0-9]*$") || Regex.IsMatch(user.Password, @"^[a-zA-Z]*$") ||
            //    Regex.IsMatch(user.Password,
            //        "^[ \\[ \\] \\^ \\-_*×――(^)$%~!＠@＃#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;/\'\"{}（）‘’“”-]*$"))
            //{
            //    HandyControl.Controls.MessageBox.Show("用户密码至少包含数字、字母和特殊字符其中两种", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            user.UserPassword = Encryption.Encrypt(user.UserPassword);

            var result = await _container.Resolve<ISystemUser>().InsertUserInfo(user);
            if (result)
            {
                HandyControl.Controls.MessageBox.Show("添加成功，跳转至登录页面", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

       
                Application.Current.Shutdown();
            }
            else
            {
                HandyControl.Controls.MessageBox.Show("添加失败", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
