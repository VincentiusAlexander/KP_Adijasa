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

namespace Aplikasi_Penitipan_Abu.Transaksi.PengambilanAbu
{
    /// <summary>
    /// Interaction logic for TandaTerimaPengambilanAbuFix.xaml
    /// </summary>
    public partial class TandaTerimaPengambilanAbuFix : Window
    {
        public TandaTerimaPengambilanAbuFix(tandaTerimaPengambilanAbuData data)
        {
            InitializeComponent();
            nama_penanggung_jawab.Text = data.nama_penanggung_jawab;
            alamat_penanggung_jawab.Text = data.alamat_penanggung_jawab;
            no_telp_penanggung_jawab.Text = data.no_telp_penanggung_jawab;
            nama_abu.Text = data.nama_abu;
            jenis_kelamin_abu.Text = data.jenis_kelamin_abu;
            tanggal_wafat.Text = data.tanggal_wafat;
            tanggal_kremasi.Text = data.tanggal_kremasi;
            pernyataan_no_kotak.Text = $"Pihak Keluarga (penanggung jawab) menyatakan bahwa abu titipan di kotak no.({data.no_kotak}) tersebut telah kami terima dengan baik";
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            btn.Visibility = Visibility.Hidden;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(this, "Pengambilan Abu");
            }
            btn.Visibility = Visibility.Visible;
        }
    }
}
