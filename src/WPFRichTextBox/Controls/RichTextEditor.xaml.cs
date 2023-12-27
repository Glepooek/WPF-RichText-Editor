using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
            if (selectedText != null)
            {
                if (selectedText.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Strikethrough)
                {
                    // 如果已经有了删除线，那么移除
                    selectedText.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                }
                else
                {
                    // 如果没有删除线，那么添加
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
        }

        private void mainRTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlainText = GetPlainText();
        }

        #endregion

        #region Methods

        public void IncreaseFontSize(double fontSize)
        {
            EditingCommands.IncreaseFontSize.Execute(fontSize, this.mainRTB);
        }

        public void DecreaseFontSize(double fontSize)
        {
            EditingCommands.DecreaseFontSize.Execute(fontSize, this.mainRTB);
        }

        private string GetPlainText()
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                this.mainRTB.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                this.mainRTB.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }

        #endregion
    }
}
