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

namespace Aplikasi_Penitipan_Abu.Transaksi.PembayaranSewa
{
    /// <summary>
    /// Interaction logic for PembayaranSewa_Add.xaml
    /// </summary>
    public partial class PembayaranSewa_Add : Page
    {
        MySqlConnection conn;
        Registrasi registrasi;
        int selectedId = -1;
        public PembayaranSewa_Add()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        private void cari_registrasi_Click(object sender, RoutedEventArgs e)
        {
            PencarianRegistrasi pencarian = new PencarianRegistrasi();
            pencarian.ShowDialog();
            this.selectedId = pencarian.selectedId;
            if (selectedId == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            MySqlCommand cmd = new MySqlCommand("select * from penitipan p left join data_abu da on p.data_abu_id = da.id where p.id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", selectedId);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            registrasi = new Registrasi();
            while (reader.Read())
            {
                registrasi.id_kotak = reader.GetInt32(4);
                registrasi.id_abu = reader.GetInt32(5);
                registrasi.nama_abu = reader.GetString(9);
            }
            reader.Close();
            if (registrasi.id_kotak == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            cmd.CommandText = "SELECT * FROM kotak LEFT JOIN kategori ON kotak.kategori_id = kategori.id WHERE kotak.id = ?idKotak";
            cmd.Parameters.AddWithValue("?idKotak", registrasi.id_kotak);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                registrasi.no_kotak = reader.GetString(2);
                registrasi.harga_kotak = reader.GetInt32(6);
            }

            reader.Close();
            registrasi.idRegistrasi = selectedId;
            no_kotak.Text = registrasi.no_kotak;
            no_registrasi.Text = registrasi.idRegistrasi.ToString();
            nama_abu.Text = registrasi.nama_abu;

            conn.Close();
        }
    }
}
