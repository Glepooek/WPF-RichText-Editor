using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace WPFRichTextBox.Example
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.sampleEditor.IncreaseFontSize(1, 24);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.sampleEditor.DecreaseFontSize(1, 14);
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
