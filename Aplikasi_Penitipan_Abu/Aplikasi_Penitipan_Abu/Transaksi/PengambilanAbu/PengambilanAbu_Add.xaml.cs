using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using MySql.Data.MySqlClient;

namespace Aplikasi_Penitipan_Abu.Transaksi.PengambilanAbu
{
    /// <summary>
    /// Interaction logic for PengambilanAbu_Add.xaml
    /// </summary>
    public partial class PengambilanAbu_Add : Page
    {
        MySqlConnection conn;
        int id_registrasi = -1;
        public PengambilanAbu_Add()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        private void cari_registrasi_Click(object sender, RoutedEventArgs e)
        {
            PencarianRegistrasi pencarianRegistrasi = new PencarianRegistrasi();
            pencarianRegistrasi.ShowDialog();
            id_registrasi = pencarianRegistrasi.selectedId;
            if (id_registrasi == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("select * from penitipan where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_registrasi);
            conn.Close();
            conn.Open();
            
        }
    }
}
