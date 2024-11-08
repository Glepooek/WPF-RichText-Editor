﻿using System;
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
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.DataContext = new PageViewModel();
        }

		private void hideToolbar_Checked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsToolBarVisible = false;
		}

		private void disableContextMenu_Checked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsContextMenuEnabled = false;
		}

		private void setReadOnly_Checked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsReadOnly = true;
		}

		private void hideToolbar_Unchecked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsToolBarVisible = true;
		}

		private void disableContextMenu_Unchecked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsContextMenuEnabled = true;
		}

		private void setReadOnly_Unchecked(object sender, RoutedEventArgs e)
		{
			this.sampleEditor.IsReadOnly = false;
		}
    }

    #region PageViewModel
    public class PageViewModel : ObservableBase
    {  
        public DelegateCommand GetXamlCommand { get; private set; }
		public DelegateCommand LoadHtmlCommand { get; private set; }
        public DelegateCommand GetPlainTextCommand { get; private set; }

        #region Constructor
        public PageViewModel()
        {
            GetXamlCommand = new DelegateCommand(() =>
            {
                MessageBox.Show(this.HtmlText);
            });

			LoadHtmlCommand = new DelegateCommand(() =>
			{
                //this.HtmlText = Properties.Resources.HTMLPage1;
                this.HtmlText = Properties.Resources.HTMLPage2;
            });

            GetPlainTextCommand = new DelegateCommand(() => 
            {
                MessageBox.Show(this.PlainText);
            });
        }
        #endregion
        
        #region Name
        private string htmlText = string.Empty;
        public string HtmlText
        {
            get
            {
                return htmlText;
            }
            set
            {
                htmlText = value;
                this.RaisePropertyChanged(p => p.HtmlText);
            }
        }

        private string plainText = string.Empty;
        public string PlainText
        {
            get
            {
                return plainText;
            }
            set
            {
                plainText = value;
                this.RaisePropertyChanged(p => p.PlainText);
            }
        }
        #endregion
    }
    #endregion

    #region Observable
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public static class ObservableBaseEx
    {
        public static void RaisePropertyChanged<T, TProperty>(this T observableBase, Expression<Func<T, TProperty>> expression) where T : ObservableBase
        {
            observableBase.RaisePropertyChanged(observableBase.GetPropertyName(expression));
        }

        public static string GetPropertyName<T, TProperty>(this T owner, Expression<Func<T, TProperty>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                var unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;

                    if (memberExpression == null)
                        throw new NotImplementedException();
                }
                else
                    throw new NotImplementedException();
            }

            var propertyName = memberExpression.Member.Name;
            return propertyName;
        }
    }
    #endregion
}
