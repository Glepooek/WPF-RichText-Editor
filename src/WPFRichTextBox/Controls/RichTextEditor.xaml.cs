using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WPFRichTextBox
{
    /// <summary>
    /// Interaction logic for BindableRichTextbox.xaml
    /// </summary>
    public partial class RichTextEditor : UserControl
    {
        #region Dp

        /// <summary></summary>
        public string HtmlText
        {
            get { return GetValue(HtmlTextProperty) as string; }
            set
            {
                SetValue(HtmlTextProperty, value);
            }
        }

        /// <summary></summary>
        public static readonly DependencyProperty HtmlTextProperty =
          DependencyProperty.Register("HtmlText", typeof(string), typeof(RichTextEditor),
          new PropertyMetadata(string.Empty));

        /// <summary></summary>
        public string PlainText
        {
            get { return GetValue(PlainTextProperty) as string; }
            set
            {
                SetValue(PlainTextProperty, value);
            }
        }

        /// <summary></summary>
        public static readonly DependencyProperty PlainTextProperty =
          DependencyProperty.Register("PlainText", typeof(string), typeof(RichTextEditor),
          new PropertyMetadata(string.Empty));

        /// <summary></summary>
        public bool IsReadOnly
        {
            get { return (GetValue(IsReadOnlyProperty) as bool? == true); }
            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        /// <summary></summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
          DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RichTextEditor),
          new PropertyMetadata(false, OnIsReadOnlyPropertyChanged));

        private static void OnIsReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RichTextEditor;
            if (e.NewValue is bool result && result)
            {
                control.tools.Visibility = Visibility.Collapsed;
                control.tools1.Visibility = Visibility.Collapsed;
                control.mainRTB.BorderBrush = new SolidColorBrush(Colors.Transparent);
                return;
            }

            control.tools.Visibility = Visibility.Visible;
            control.tools1.Visibility = Visibility.Visible;
        }

        #endregion

        #region Constructor

        /// <summary></summary>
        public RichTextEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region EventHandlers

        private void StrikethroughButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.mainRTB == null || this.mainRTB.Selection == null)
            {
                return;
            }

            TextRange selectedText = new TextRange(mainRTB.Selection.Start, mainRTB.Selection.End);
            if (selectedText != null && !selectedText.IsEmpty)
            {
                if (selectedText.GetPropertyValue(Inline.TextDecorationsProperty) is TextDecorationCollection textDecorations)
                {
                    var clone = textDecorations.Clone();
                    var textDecoration = clone.FirstOrDefault(td => td.Location == TextDecorationLocation.Strikethrough);
                    if (textDecoration != null)
                    {
                        clone.Remove(textDecoration);
                    }
                    else
                    {
                        var myTextDecoration = new TextDecoration() { Location = TextDecorationLocation.Strikethrough };                      
                        clone.Add(myTextDecoration);
                    }

                    selectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, clone);
                }
                else
                {
                    selectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
                }
            }
        }

        private void ColorCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.mainRTB == null || this.mainRTB.Selection == null)
            {
                return;
            }

            var colorStr = (e.AddedItems[0] as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrEmpty(colorStr)) return;
            Color color = (Color)ColorConverter.ConvertFromString(colorStr);
            var brush = new SolidColorBrush(color);
            this.mainRTB.Selection.ApplyPropertyValue(ForegroundProperty, brush);
            this.mainRTB.Focus();
        }

        private void FontSizeCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.mainRTB == null || this.mainRTB.Selection == null)
            {
                return;
            }

            double fontSize = 14;
            switch (FontSizeCmb.SelectedIndex)
            {
                case 0: // 小
                    fontSize = 14;
                    break;
                case 1: // 中
                    fontSize = 20;
                    break;
                case 2: // 大
                    fontSize = 30;
                    break;
                default:
                    break;
            }

            this.mainRTB.Selection.ApplyPropertyValue(FontSizeProperty, fontSize);
            this.mainRTB.Focus();
        }

        private void mainRTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlainText = GetPlainText();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 增大字体
        /// </summary>
        /// <param name="fontSize"></param>
        /// <param name="maxFontSize"></param>
        public void IncreaseFontSize(double fontSize, double maxFontSize)
        {
            var currentFontSize = this.mainRTB.FontSize;
            if (currentFontSize >= maxFontSize)
            {
                return;
            }
            currentFontSize += fontSize;
            this.mainRTB.FontSize = currentFontSize;
        }

        /// <summary>
        /// 减小字体
        /// </summary>
        /// <param name="fontSize"></param>
        ///  <param name="minFontSize"></param>
        public void DecreaseFontSize(double fontSize, double minFontSize)
        {
            var currentFontSize = this.mainRTB.FontSize;
            if (minFontSize < 0 || currentFontSize <= minFontSize)
            {
                return;
            }
            currentFontSize -= fontSize;
            this.mainRTB.FontSize = currentFontSize;
        }

        private string GetPlainText()
        {
            TextRange textRange = new TextRange(this.mainRTB.Document.ContentStart, this.mainRTB.Document.ContentEnd);
            return textRange.Text;
        }

        #endregion
    }
}
