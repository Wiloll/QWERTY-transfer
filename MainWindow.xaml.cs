using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace QWERTY_Transfer
{
    public partial class MainWindow : Window
    {
        public string[] qwerty_UA = { "й", "ц", "у", "к", "е", "н", "г", "ш", "щ", "з", "х", "ї", "ф", "і", "в", "а", "п", "р", "о", "л", "д", "ж", "є", "я", "ч", "с", "м", "и", "т", "ь", "б", "ю" };
        public string[] qwerty_EN = { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "z", "x", "c", "v", "b", "n", "m", ",", "." };

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Change(object sender, RoutedEventArgs e)
        {
            OutputTextBlock.Text = (OutputTextBlock.Text == "EN to UA") ? "UA to EN" : "EN to UA";
        }

        private void Trans(object sender, RoutedEventArgs e)
        {
            string text = InputTextBox.Text;
            string result = ConvertText(text, OutputTextBlock.Text == "EN to UA");

            CopyToClipboard(result);

            MessageBox.Show("Текст скопійовано: " + result);
        }

        private string ConvertText(string input, bool enToUa)
        {
            string[] source = enToUa ? qwerty_EN : qwerty_UA;
            string[] target = enToUa ? qwerty_UA : qwerty_EN;

            return string.Concat(input.Select(c =>
            {
                int index = Array.IndexOf(source, c.ToString());
                return index != -1 ? target[index] : c.ToString();
            }));
        }

        private void CopyToClipboard(string text)
        {
            int retryCount = 5; 
            int delay = 100; 

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        Clipboard.Clear();
                        Clipboard.SetText(text); 
                    });
                    return;
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    Thread.Sleep(delay);
                }
            }

            MessageBox.Show("Не вдалося скопіювати текст. Спробуйте ще раз.");
        }
    }
}
