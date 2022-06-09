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

namespace Aplikasi_Penitipan_Abu.Transaksi.PembayaranJaminan
{
    /// <summary>
    /// Interaction logic for TandaTerimaPembayaranJaminanFix.xaml
    /// </summary>
    public partial class TandaTerimaPembayaranJaminanFix : Window
    {
        public TandaTerimaPembayaranJaminanFix(tandaTerimaPembayaranJaminanData data)
        {
            InitializeComponent();
            no_kwitansi.Text = data.no_kwitansi;
            no_registrasi.Text = data.no_registrasi;
            tanggal_pembayaran.Text = data.tanggal_pembayaran;
            nama_abu.Text = data.nama_abu;
            nama_penanggung_jawab.Text = data.nama_penanggung_jawab;
            tanggal_tanda_tangan.Text += " "+ data.tanggal_pembayaran;
            int uang = Convert.ToInt32(data.uang_jaminan.Split('.')[1]);
            uang_rupiah.Text = uang.ToString();
            banyaknya_uang.Text = pembuatan_bilangan.Terbilang(Convert.ToInt64(uang));
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            btnPrint.Visibility = Visibility.Hidden;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "Pembayaran Jaminan");
            }
            btnPrint.Visibility = Visibility.Visible;
        }
    }
}
