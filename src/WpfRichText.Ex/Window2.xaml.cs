using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WpfRichText
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            this.DataContext = new PageViewModel();
        }

		private void setReadOnly_Checked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsReadOnly = true;
		}

		private void setReadOnly_Unchecked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsReadOnly = false;
		}
    }
}
