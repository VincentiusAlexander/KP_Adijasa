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

namespace Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu
{
    /// <summary>
    /// Interaction logic for TandaTerimaPenitipanAbuFix.xaml
    /// </summary>
    public partial class TandaTerimaPenitipanAbuFix : Window
    {
        public TandaTerimaPenitipanAbuFix(String nama, String alamat, String notelp, String kotak, String tanggal_registrasi, String namaAbu, String jk, String tanggal_meninggal, String tanggal_kremasi)
        {
            InitializeComponent();
            tb_Nama_Penanggung_Jawab.Text = nama;
            tb_Alamat_Penanggung_Jawab.Text = alamat;
            tb_Notelp_Penanggung_Jawab.Text = notelp;
            tb_Keterangan.Text = "Pihak Yayasan Adi Jasa tidak keberatan abu tersebut ditempatkan di kotak no. " + kotak + " dengan syarat pihak keluarga (penanggung jawab) harus menaati tata tertib Yayasan Adi Jasa";
            tb_Tempat_Tanggal_Registrasi.Text = "Surabaya, " + tanggal_registrasi;
            tb_Nama_Abu.Text = namaAbu;
            tb_jk.Text = jk;
            tb_tgl_meninggal.Text = tanggal_meninggal;
            tb_tgl_kremasi.Text = tanggal_kremasi;
            tb_penjelasan.Text = "Dengan ini menerangkan bahwa pada tanggal " + tanggal_registrasi + " telah menitipkan abu :";
        }

        private void btn_print_tanda_terima_penitipan_abu_Click(object sender, RoutedEventArgs e)
        {
            btn_print_tanda_terima_penitipan_abu.Visibility = Visibility.Hidden;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "Tanda Terima Penitipan Abu");
            }
            btn_print_tanda_terima_penitipan_abu.Visibility = Visibility.Visible;
        }
    }
}
