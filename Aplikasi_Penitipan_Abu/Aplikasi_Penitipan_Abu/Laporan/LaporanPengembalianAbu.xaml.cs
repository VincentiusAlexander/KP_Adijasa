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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aplikasi_Penitipan_Abu.Laporan
{
    /// <summary>
    /// Interaction logic for LaporanPengembalianAbu.xaml
    /// </summary>
    public partial class LaporanPengembalianAbu : Page
    {
        public LaporanPengembalianAbu()
        {
            InitializeComponent();
        }

        private void btn_filter_Click(object sender, RoutedEventArgs e)
        {
            report_pengambilan_abu rpa = new report_pengambilan_abu();
            try
            {
                rpa.SetParameterValue("tanggal_awal", tanggal_awal.SelectedDate);
                rpa.SetParameterValue("tanggal_akhir", tanggal_akhir.SelectedDate);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Periksa Kembali Tanggal Yang Telah Di Inputkan", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            creport.ViewerCore.ReportSource = rpa;
        }
    }
}
