using Microsoft.Win32;
using System;
using System.Windows;

namespace JaneczkoSklep
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            sqlCon.IfDatabaseExist(baza);
            sqlCon.CloseConnection();
        }

        public string baza = "BazaSklep.sqlite";
        public ImageDecoder imageDecoder = new ImageDecoder();
        public SqlCon sqlCon = new SqlCon();
        private void WczytajBtn_Click(object sender, RoutedEventArgs e)
        {
            ZdjecieText.Text = OpenFile();
        }
        private string OpenFile()
        {
            string filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Pliki obrazu (*.png;*.jpeg;*.jpg)|*png;*.jpeg;*.jpg|Wszystkie pliki (*.*)|*.*",
                Title = "Wskaż obraz",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }

        private void DodajDoBazyBtn_Click(object sender, RoutedEventArgs e)
        {
            int Ilosc = 0;
            double Cena = 0.0;
            string Nazwa = NazwaText.Text;
            string convertedImage = imageDecoder.SaveAsBase64(ZdjecieText.Text);
            int.TryParse(IloscText.Text, out Ilosc);
            double.TryParse(CenaText.Text, out Cena);
            string CenaP = Cena.ToString() + " zł";

            if (Nazwa != "" && Ilosc != 0 && Cena != 0.0 && convertedImage != null)
            {
                try
                {
                    sqlCon.IfDatabaseExist(baza);
                    sqlCon.AddToSql(Nazwa, Ilosc.ToString(), CenaP, convertedImage);
                    sqlCon.CloseConnection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd\n" + ex.ToString(), "info", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Dodano do bazy", "info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Błąd\nWszystkie pola muszą być zapełnione");
                return;
            }
        }
        private void WyswietlBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayShop displayShop = new DisplayShop();
            displayShop.Show();
        }
    }
}