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

namespace Aplikasi_Penitipan_Abu
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        int role;
        public Menu()
        {
            InitializeComponent();
        }
        public Menu(int role)
        {
            InitializeComponent();
            this.role = role;
            mainFrame.Content = new Overview();
        }

        private void penitipan_abu_add_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu.PenitipanAbu_Add();
        }

        private void penitipan_abu_edit_Click(object sender, RoutedEventArgs e)
        {
            if(role == 0)
            {
                System.Windows.Forms.MessageBox.Show("Role Bukan Admin", "Unauthorized", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu.PenitipanAbu_Edit();
        }

        private void pembayaran_sewa_add_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PembayaranSewa.PembayaranSewa_Add();
        }

        private void pembayaran_sewa_edit_Click(object sender, RoutedEventArgs e)
        {
            if (role == 0)
            {
                System.Windows.Forms.MessageBox.Show("Role Bukan Admin", "Unauthorized", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PembayaranSewa.PembayaranSewa_Edit();
        }

        private void pembayaran_perpanjangan_add_Click(object sender, RoutedEventArgs e)
        {
            //pembayaran perpanjangan wi pie, masuk nde mana :v
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PerpanjanganSewa.PerpanjanganSewa_Add();
        }

        private void pembayaran_perpanjangan_edit_Click(object sender, RoutedEventArgs e)
        {
            if (role == 0)
            {
                System.Windows.Forms.MessageBox.Show("Role Bukan Admin", "Unauthorized", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PerpanjanganSewa.PerpanjanganSewa_Edit();
        }

        private void pembayaran_jaminan_add_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PembayaranJaminan.PembayaranJaminan_Add();
        }

        private void pembayaran_jaminan_edit_Click(object sender, RoutedEventArgs e)
        {
            if (role == 0)
            {
                System.Windows.Forms.MessageBox.Show("Role Bukan Admin", "Unauthorized", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PembayaranJaminan.PembayaranJaminan_Edit();
        }


        private void pengembalian_abu_edit_Click(object sender, RoutedEventArgs e)
        {
            if (role == 0)
            {
                System.Windows.Forms.MessageBox.Show("Role Bukan Admin", "Unauthorized", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PengambilanAbu.PengambilanAbu_Edit();
        }

        private void pengembalian_abu_add_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Transaksi.PengambilanAbu.PengambilanAbu_Add();
        }

        private void master_kotak_add_Click(object sender, RoutedEventArgs e)
        {
            if (role == 0)
            {
                System.Windows.Forms.MessageBox.Show("Role Bukan Admin", "Unauthorized", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Master.MasterKotak();
        }

        private void master_kategori_add_Click(object sender, RoutedEventArgs e)
        {
            if (role == 0)
            {
                System.Windows.Forms.MessageBox.Show("Role Bukan Admin", "Unauthorized", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Master.MasterKategori();
        }

        private void laporan_penerimaan_abu_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Laporan.LaporanPenerimaanAbu();
        }

        private void laporan_pembayaran_abu_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Laporan.LaporanPembayaranAbu();
        }

        private void laporan_pengambilan_abu_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Laporan.LaporanPengembalianAbu();
        }

        private void laporan_jaminan_abu_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Laporan.LaporanJaminanAbu();
        }

        private void laporan_piutang_abu_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Aplikasi_Penitipan_Abu.Laporan.LaporanPiutang();
        }

        private void overview_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Overview();
        }
    }
}
