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
    /// Interaction logic for PengambilanAbu_Edit.xaml
    /// </summary>
    public partial class PengambilanAbu_Edit : Page
    {
        MySqlConnection conn;
        int id_pengambilan_abu = -1;
        int id_registrasi = -1;
        int id_penanggung_jawab = -1;
        int id_data_abu = -1;
        string nama_penanggung_jawab = "";
        string alamat_penanggung_jawab = "";
        string nomor_telp_penanggung_jawab = "";
        string nama_abu = "";
        string jenis_kelamin_abu = "";
        DateTime tanggal_wafat_abu;
        DateTime tanggal_kremasi_abu;
        string no_kotak = "";

        public PengambilanAbu_Edit()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        private void cari_pengambilan_abu_Click(object sender, RoutedEventArgs e)
        {
            PencarianPengembalianAbu pencarianRegistrasi = new PencarianPengembalianAbu();
            pencarianRegistrasi.ShowDialog();
            id_pengambilan_abu = pencarianRegistrasi.selectedId;
            if (id_pengambilan_abu == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            MySqlCommand cmd = new MySqlCommand("select * from pengambilan_abu where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_pengambilan_abu);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_registrasi = reader.GetInt32(1);
                no_pengambilan_abu_txt.Text = reader.GetString(4);
            }
            reader.Close();
            conn.Close();
            conn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = "select * from penitipan where id = ?id";
            cmd.Parameters.AddWithValue("?id", id_registrasi);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_data_abu = reader.GetInt32(5);
                id_penanggung_jawab = reader.GetInt32(6);
                no_registrasi_txt.Text = reader.GetString(9);
            }
            reader.Close();
            conn.Close();
            conn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = "select * from penanggung_jawab where id = ?id";
            cmd.Parameters.AddWithValue("?id", id_penanggung_jawab);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nama_penanggung_jawab = reader.GetString(1);
                alamat_penanggung_jawab = reader.GetString(2);
                nomor_telp_penanggung_jawab = reader.GetString(3);
            }
            nama_penanggung_jawab_txt.Text = nama_penanggung_jawab;
            alamat_penanggung_jawab_txt.Text = alamat_penanggung_jawab;
            nomor_penanggung_jawab_txt.Text = nomor_telp_penanggung_jawab;

            reader.Close();
            conn.Close();
            conn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = "select * from data_abu where id = ?id";
            cmd.Parameters.AddWithValue("?id", id_data_abu);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nama_abu = reader.GetString(1);
                jenis_kelamin_abu = reader.GetString(4);
                tanggal_wafat_abu = reader.GetDateTime(6);
                tanggal_kremasi_abu = reader.GetDateTime(7);
            }
            nama_abu_txt.Text = nama_abu;
            jenis_kelamin_abu_txt.Text = jenis_kelamin_abu;
            tanggal_wafat_abu_txt.Text = tanggal_wafat_abu.ToString("dd/MM/yyyy");
            tanggal_kremasi_abu_txt.Text = tanggal_kremasi_abu.ToString("dd/MM/yyyy");
            reader.Close();
            conn.Close();
            conn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = "select k.no_kotak from penitipan p left join kotak k on k.id = p.kotak_id where p.id = ?id";
            cmd.Parameters.AddWithValue("?id", id_registrasi);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                no_kotak = reader.GetString(0);
            }
            no_kotak_abu_txt.Text = no_kotak;

            conn.Close();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (id_pengambilan_abu == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pilih dulu data yang ingin dihapus", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("update pengambilan_abu set status = 1 where id = ?id_pengambilan_abu", conn);
            cmd.Parameters.AddWithValue("?id_pengambilan_abu", id_pengambilan_abu);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            System.Windows.Forms.MessageBox.Show("Penghapusan pengambilan abu Berhasil !", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            no_pengambilan_abu_txt.Text = "-";
            alamat_penanggung_jawab_txt.Text = "-";
            jenis_kelamin_abu_txt.Text = "-";
            nama_abu_txt.Text = "-";
            nama_penanggung_jawab_txt.Text = "-";
            nomor_penanggung_jawab_txt.Text = "-";
            no_kotak_abu_txt.Text = "-";
            no_registrasi_txt.Text = "-";
            tanggal_kremasi_abu_txt.Text = "-";
            tanggal_wafat_abu_txt.Text = "-";
            id_registrasi = -1;
            id_pengambilan_abu = -1;
        }

        private void btnCetakTandaTerima_Click(object sender, RoutedEventArgs e)
        {
            if (id_registrasi == -1)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan Pencarian Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            try
            {
                TandaTerimaPengambilanAbuFix tandaTerima = new TandaTerimaPengambilanAbuFix(new tandaTerimaPengambilanAbuData(nama_penanggung_jawab, alamat_penanggung_jawab, nomor_telp_penanggung_jawab, nama_abu, no_kotak, jenis_kelamin_abu, tanggal_wafat_abu.ToString("dd/MM/yyyy"), tanggal_kremasi_abu.ToString("dd/MM/yyyy")));
                tandaTerima.Show();
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan Pencarian Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            no_pengambilan_abu_txt.Text = "-";
            alamat_penanggung_jawab_txt.Text = "-";
            jenis_kelamin_abu_txt.Text = "-";
            nama_abu_txt.Text = "-";
            nama_penanggung_jawab_txt.Text = "-";
            nomor_penanggung_jawab_txt.Text = "-";
            no_kotak_abu_txt.Text = "-";
            no_registrasi_txt.Text = "-";
            tanggal_kremasi_abu_txt.Text = "-";
            tanggal_wafat_abu_txt.Text = "-";
            id_registrasi = -1;
            id_pengambilan_abu = -1;
        }
    }
}
