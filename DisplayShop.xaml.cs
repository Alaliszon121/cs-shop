using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace JaneczkoSklep
{
    public partial class DisplayShop : Window
    {
        public DisplayShop()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlCon sqlCon = new SqlCon();
            lvDataBinding.ItemsSource = sqlCon.getFromTable("BazaSklep.sqlite");
        }

        private void LvDataBinding_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Produkt element = (Produkt)lvDataBinding.SelectedItems[0];
            string nazwa = element.Name;
            string cena = element.Price;
            string msg = string.Format("Wybrano pozycję:\t{0}\nCena:\t{1}", nazwa, cena);
            MessageBox.Show(msg, "Produkt", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class Produkt
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public BitmapSource BitmapSource { get; set; }
    }
}
