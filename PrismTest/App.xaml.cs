using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BLL.IService;
using Dao.CodeFirst;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using PrismTest.Views;
using PrismTest.Views.Login;
using SqlSugar;
using SqlSugar.IOC;
using UtilityLibrary.Extend;

namespace PrismTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hWnd);
        Mutex AppMutex;
        public App()
        {
            //UI线程未捕获异常处理事件
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            LogHelper.Instance.Error("Task线程异常：" + e.Exception.InnerException?.Message +
                                     e.Exception.InnerException?.StackTrace);
            e.SetObserved(); //设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }

        private void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出      
                LogHelper.Instance.Error("UI线程异常:" + e.Exception.Message + e.Exception.StackTrace);
            }
            catch (Exception)
            {
                //此时程序出现严重异常，将强制结束退出
                LogHelper.Instance.Error("UI线程发生致命错误！");
            }
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("非UI线程发生致命错误");
            }

            sbEx.Append("非UI线程异常：");
            if (e.ExceptionObject is Exception exception)
            {
                sbEx.Append(exception.Message + exception.StackTrace);
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }

            LogHelper.Instance.Error(sbEx.ToString());
        }

        /// <summary>
        /// prism Ioc注入
        /// </summary>
        /// <param name="containerRegistry"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginContentView>();
            RegisterAssemblyTypes(containerRegistry);
        }

        /// <summary>
        /// 启动窗口
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Window CreateShell()
        {
            var result = Container.Resolve<ISystemUser>().SearchUserInfo().Result;
            if (result.Any())
            {
                return Container.Resolve<LoginWindow>();
            }
            else
            {
                return Container.Resolve<AddUserWindow>();
            }

            //throw new NotImplementedException();
        }

        /// <summary>
        /// IOC批量注册  接口及对应实现类
        /// </summary>
        /// <param name="containerRegistry"></param>
        private void RegisterAssemblyTypes(IContainerRegistry containerRegistry)
        {
            try
            {
                foreach (var interfaceType in Assembly.Load("BLL.IService").GetTypes())
                {
                    if (!interfaceType.Name.StartsWith("I", StringComparison.CurrentCultureIgnoreCase)) continue;
                    foreach (var serviceType in Assembly.Load("BLL.IServiceImpl").GetTypes())
                    {
                        if (!serviceType.Name.EndsWith("Impl", StringComparison.CurrentCultureIgnoreCase)) continue;
                        if (interfaceType.Name.Remove(0, 1)
                            .Equals(serviceType.Name.Substring(0, serviceType.Name.Length - 4),
                                StringComparison.CurrentCultureIgnoreCase))
                        {
                            containerRegistry.RegisterSingleton(interfaceType, serviceType);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error(ex.Message+ex.StackTrace);
            }
           
        }


        protected override async void OnStartup(StartupEventArgs e)
        {
            AppMutex = new Mutex(true, "TestAppName", out var createdNew);
            if (!createdNew)
            {
                var current = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id == current.Id) continue;
                    SetForegroundWindow(process.MainWindowHandle);
                    break;
                }
                Shutdown();
            }
            //添加IServiceCollection容器
            PrismContainerExtension.Current.RegisterServices(s =>
            {
                s.AddSqlSugar(new IocConfig()
                {
                    ConnectionString = " DataSource=" +Directory.GetCurrentDirectory()+ "\\DataBase\\fkh_truckscales.sqlite",
                  
                    DbType = IocDbType.Sqlite,
                    IsAutoCloseConnection = true
                });
                s.ConfigurationSugar(db =>
                {
                    db.Aop.OnError = exp =>
                    {
                        LogHelper.Instance.Error(exp.Sql);
                    };
                    db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        EntityService = (c, p) =>
                        {
                            if (c.PropertyType.IsGenericType &&
                                c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                p.IsNullable = true;
                            }
                        }
                    };
                });
                s.AddAutoMapper(typeof(MapperProfiles).Assembly);
            });
            new DatabaseInit().Init();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
