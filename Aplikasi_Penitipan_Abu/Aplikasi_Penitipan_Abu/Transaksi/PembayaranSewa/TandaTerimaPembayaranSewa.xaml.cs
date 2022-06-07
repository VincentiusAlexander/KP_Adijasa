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
using System.Printing;

namespace Aplikasi_Penitipan_Abu.Transaksi.PembayaranSewa
{
    /// <summary>
    /// Interaction logic for TandaTerimaPembayaranSewa.xaml
    /// </summary>
    public partial class TandaTerimaPembayaranSewa : Window
    {
        public TandaTerimaPembayaranSewa(tandaTerimaPembayaranSewaData data)
        {
            InitializeComponent();
            noKwitansi.Text = data.no_kwitansi.ToString();
            noRegistrasi.Text = data.no_registrasi.ToString();
            noKotak.Text = data.no_kotak.ToString();
            tanggalSekarang.Text = data.tanggal_pembayaran;
            noKotak.Text = data.no_kotak;
            NamaAbu.Text = data.nama_abu;
            tempat_tanggal.Text = "Surabaya, " + data.tanggal_pembayaran;
            keterangan.Text = "Sudah terima dari " + data.penanggung_jawab +" untuk periode "+ data.jangka_waktu;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn.Visibility = Visibility.Hidden;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "Pembayaran Sewa");
            }
            btn.Visibility = Visibility.Visible;
        }
    }
}
