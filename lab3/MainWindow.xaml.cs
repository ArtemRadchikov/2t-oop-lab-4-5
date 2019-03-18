using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Documents;
using System.IO;
using Microsoft.Win32;
using Path = System.IO.Path;
using System.Windows.Resources;

namespace lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 8; i <= 12; i += 1)
                TextSize.Items.Add((double)i);

            for (int i = 14; i <= 28; i += 2)
                TextSize.Items.Add((double)i);

            TextSize.Items.Add(36.0);
            TextSize.Items.Add(48.0);
            TextSize.Items.Add(72.0);

            TextSize.SelectedItem = 12.0;
            richTextBox.FontSize = 12;
            richTextBox.FontFamily = new FontFamily("Calibri");
            FontFamilyComb.Text = "Calibri";

            CommandBinding commandCut = new CommandBinding();

            commandCut.Command = ApplicationCommands.Cut;
            commandCut.Executed += Click_Cut;
            Cut.CommandBindings.Add(commandCut);
            // KCut.CommandBindings.Add(commandCut);

        }

      

        private void Click_Copy(object sender, RoutedEventArgs e)
        {
            richTextBox.Copy();
        }

        private void Click_Insert(object sender, RoutedEventArgs e)
        {
            richTextBox.Paste();
        }

        private void Click_Cut(object sender, RoutedEventArgs e)
        {
            richTextBox.Cut();
        }

        private void TS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            richTextBox.Selection.ApplyPropertyValue(System.Windows.Controls.RichTextBox.FontSizeProperty, TextSize.SelectedItem);
            richTextBox.Focus();
        }

        private void ColorCheng(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog MyDialog = new System.Windows.Forms.ColorDialog();
            if (MyDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ColorButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(MyDialog.Color.A, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B));
                richTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(System.Windows.Media.Color.FromArgb(MyDialog.Color.A, MyDialog.Color.R, MyDialog.Color.G, MyDialog.Color.B)));
                richTextBox.Focus();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = richTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty);

            if (s.ToString() == "Normal")
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            else
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

            richTextBox.Focus();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = richTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty);

            if (s.ToString() == FontStyles.Normal.ToString())
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            else
                richTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);

            richTextBox.Focus();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = richTextBox.Selection.GetPropertyValue(TextBlock.TextProperty);
            System.Windows.MessageBox.Show(s.ToString());
            if (s.ToString() == TextDecorations.OverLine.ToString())
                richTextBox.Selection.ApplyPropertyValue(TextBlock.TextDecorationsProperty, TextDecorations.Underline);
            else
                richTextBox.Selection.ApplyPropertyValue(TextBlock.TextDecorationsProperty, TextDecorations.OverLine);

            richTextBox.Focus();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RichText Files (*.rtf)|*.rtf|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                        doc.Load(fs, DataFormats.Rtf);
                    else if (Path.GetExtension((ofd.FileName).ToLower()) == ".txt")
                        doc.Load(fs, DataFormats.Text);
                    else
                        doc.Load(fs, DataFormats.Xaml);

                }
            }
        }

        private void newFile(object sender, RoutedEventArgs e)
        {
            richTextBox.Document.Blocks.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|RichText Files (*.rtf)|*.rtf|XAML Files (*.xaml)|*.xaml|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                        doc.Save(fs, DataFormats.Rtf);
                    else if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                        doc.Save(fs, DataFormats.Text);
                    else
                        doc.Save(fs, DataFormats.Xaml);
                }
            }
        }

        private void rtb_Changed(object sender, TextChangedEventArgs e)
        {
            TextRange text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            MessageBox.Show(text.Text.Length + " символов ");
            lenth.Text = text.Text.Length + " символов " + text.Text.Split(' ').Length + " слов";
        }
        
    }
}
