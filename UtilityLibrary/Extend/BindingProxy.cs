using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UtilityLibrary.Extend
{
    internal class BindingProxy:Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
        /// <summary>
        /// 定义依赖属性,用来解决一部分UI控件 如DataGridCoulumn(不在DataGrid的可视化树中)的样式动态变化
        /// </summary>
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
    }
}
