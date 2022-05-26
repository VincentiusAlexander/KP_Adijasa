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

        public PengambilanAbu_Add()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        private void cari_registrasi_Click(object sender, RoutedEventArgs e)
        {
            PencarianRegistrasi_pengambilan_abu pencarianRegistrasi = new PencarianRegistrasi_pengambilan_abu();
            pencarianRegistrasi.ShowDialog();
            id_registrasi = pencarianRegistrasi.selectedId;
            if (id_registrasi == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            no_registrasi_txt.Text = id_registrasi.ToString();
            
            MySqlCommand cmd = new MySqlCommand("select * from penitipan where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_registrasi);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_penanggung_jawab = reader.GetInt32(6);
                id_data_abu = reader.GetInt32(5);
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
            nomor_telp_penanggung_jawab_txt.Text = nomor_telp_penanggung_jawab;

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

        private void btn_ambil_abu_Click(object sender, RoutedEventArgs e)
        {
            if (id_registrasi == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pilih dulu registrasi yang ingin diambil", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            if (System.Windows.Forms.MessageBox.Show("Lakukan Pengambilan Abu ?","Confirm",System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                MySqlCommand command = new MySqlCommand("select count(*) from pengambilan_abu where id_penitipan = ?id_penitipan", conn);
                command.Parameters.AddWithValue("?id_penitipan", id_registrasi);
                conn.Close();
                conn.Open();
                int count = Int32.Parse(command.ExecuteScalar().ToString());
                conn.Close();
                if (count >= 1)
                {
                    MySqlCommand cmd = new MySqlCommand("update pengambilan_abu set status = 0, tanggal_pengambilan = ?tanggal_pengambilan where id_penitipan = ?id_penitipan",conn);
                    cmd.Parameters.AddWithValue("?id_penitipan", id_registrasi);
                    cmd.Parameters.AddWithValue("?tanggal_pengambilan", DateTime.Now.ToString("yyyy-MM-dd"));
                    conn.Close();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand("insert into pengambilan_abu values (0,?id_penitipan,?tanggal_pengambilan,0)", conn);
                    conn.Close();
                    conn.Open();
                    cmd.Parameters.AddWithValue("?id_penitipan", id_registrasi);
                    cmd.Parameters.AddWithValue("?tanggal_pengambilan", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                command = new MySqlCommand("select * from penitipan where id = ?id", conn);
                command.Parameters.AddWithValue("?id", id_registrasi);
                conn.Close();
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                int id_kotak = -1;
                while (reader.Read())
                {
                    id_kotak = reader.GetInt32(4);
                }
                reader.Close();
                conn.Close();
                command = new MySqlCommand("update kotak set terpakai = 0 where id = ?id", conn);
                command.Parameters.AddWithValue("?id", id_kotak);
                conn.Close();
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                command = new MySqlCommand("update pembayaran_sewa set tanggal_diambil = ?tanggal_diambil where id_penitipan = ?id_penitipan", conn);
                command.Parameters.AddWithValue("?tanggal_diambil", DateTime.Now.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("?id_penitipan", id_registrasi);
                conn.Close();
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                System.Windows.Forms.MessageBox.Show("Pengambilan Berhasil !", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                //lakukan reset tampilan
                alamat_penanggung_jawab_txt.Text = "-";
                jenis_kelamin_abu_txt.Text = "-";
                nama_abu_txt.Text = "-";
                nama_penanggung_jawab_txt.Text = "-";
                nomor_telp_penanggung_jawab_txt.Text = "-";
                no_kotak_abu_txt.Text = "-";
                no_registrasi_txt.Text = "-";
                tanggal_kremasi_abu_txt.Text = "-";
                tanggal_wafat_abu_txt.Text = "-";
            }
        }

        private void btnCetak_Click(object sender, RoutedEventArgs e)
        {
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
    }
    public class tandaTerimaPengambilanAbuData
    {
        
        public string nama_penanggung_jawab;
        public string alamat_penanggung_jawab;
        public string no_telp_penanggung_jawab;
        public string nama_abu;
        public string jenis_kelamin_abu;
        public string tanggal_wafat;
        public string tanggal_kremasi;
        public string no_kotak;
        public tandaTerimaPengambilanAbuData(string nama_penanggung_jawab, string alamat_penanggung_jawab, string no_telp_penanggung_jawab, string nama_abu, string no_kotak, string jenis_kelamin_abu, string tanggal_wafat, string tanggal_kremasi)
        {
            this.nama_penanggung_jawab = nama_penanggung_jawab;
            this.alamat_penanggung_jawab = alamat_penanggung_jawab;
            this.no_telp_penanggung_jawab = no_telp_penanggung_jawab;
            this.nama_abu = nama_abu;
            this.jenis_kelamin_abu = jenis_kelamin_abu;
            this.tanggal_wafat = tanggal_wafat;
            this.tanggal_kremasi = tanggal_kremasi;
            this.no_kotak = no_kotak;
        }
    }
}
