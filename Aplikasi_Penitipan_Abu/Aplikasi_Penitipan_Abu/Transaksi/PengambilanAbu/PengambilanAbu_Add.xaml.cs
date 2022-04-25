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

namespace Aplikasi_Penitipan_Abu.Transaksi.PengambilanAbu
{
    /// <summary>
    /// Interaction logic for PengambilanAbu_Add.xaml
    /// </summary>
    public partial class PengambilanAbu_Add : Page
    {
        public PengambilanAbu_Add()
        {
            InitializeComponent();
        }

        private void cari_registrasi_Click(object sender, RoutedEventArgs e)
        {
            PencarianRegistrasi pencarianRegistrasi = new PencarianRegistrasi();
            pencarianRegistrasi.ShowDialog();
            System.Windows.Forms.MessageBox.Show(pencarianRegistrasi.selectedId+" check");
        }
    }
}
