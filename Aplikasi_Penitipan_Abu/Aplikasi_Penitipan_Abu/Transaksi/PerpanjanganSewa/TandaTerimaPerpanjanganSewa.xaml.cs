using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Aplikasi_Penitipan_Abu.Transaksi.PerpanjanganSewa
{
    /// <summary>
    /// Interaction logic for TandaTerimaPerpanjanganSewa.xaml
    /// </summary>
    public partial class TandaTerimaPerpanjanganSewa : Window
    {
        public TandaTerimaPerpanjanganSewa(tandaTerimaPerpanjanganSewaData data)
        {
            InitializeComponent();
            noKwitansi.Text = data.no_kwitansi.ToString();
            noRegistrasi.Text = data.no_registrasi.ToString();
            noKotak.Text = data.no_kotak.ToString();
            tanggalSekarang.Text = data.tanggal_pembayaran;
            noKotak.Text = data.no_kotak;
            NamaAbu.Text = data.nama_abu;
            tempat_tanggal.Text = "Surabaya, " + data.tanggal_pembayaran;
            keterangan.Text = "Sudah terima dari, " + data.penanggung_jawab;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn_print.Visibility = Visibility.Hidden;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "Perpanjangan Sewa");
            }
            btn_print.Visibility = Visibility.Visible;
        }
    }
}
