using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Aplikasi_Penitipan_Abu
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : Page
    {
        MySqlConnection conn;
        DataTable data;
        public Overview()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            loadDataGrid(tb_pencarian.Text);
            initOverview();
        }
        public void loadDataGrid(string filter)
        {
            data = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SELECT k.no_kotak AS 'No Kotak', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', da.alamat_abu AS 'Alamat Abu', da.jenis_kelamin AS 'Jenis Kelamin',  DATE(da.tanggal_lahir) AS 'Tanggal Lahir',  DATE(da.tanggal_wafat) AS 'Tanggal Wafat', DATE(da.tanggal_kremasi) AS 'Tanggal Kremasi', da.keterangan AS 'Keterangan', pj1.nama AS 'Nama Penanggung Jawab 1', pj2.nama AS 'Nama Penanggung Jawab 2' FROM penitipan p LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN kotak k ON k.id = p.kotak_id LEFT JOIN penanggung_jawab pj1 ON p.penanggung_jawab_satu_id = pj1.id LEFT JOIN penanggung_jawab pj2 ON p.penanggung_jawab_dua_id = pj2.id LEFT JOIN pembayaran_sewa ps ON ps.id_penitipan = p.id WHERE (k.no_kotak LIKE ?a or da.nama_abu LIKE ?a or da.nama_alternatif_abu LIKE ?a or da.alamat_abu LIKE ?a or da.jenis_kelamin LIKE ?a or pj1.nama LIKE ?a or pj2.nama LIKE ?a) and ps.tanggal_akhir >= ?tanggal_akhir and ps.tanggal_awal <= ?tanggal_awal", conn);
            cmd.Parameters.AddWithValue("?a", "%" + filter + "%");
            cmd.Parameters.AddWithValue("?tanggal_akhir", DateTime.Today);
            cmd.Parameters.AddWithValue("?tanggal_awal", DateTime.Today);
            conn.Close();
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(data);
            dtGrid.ItemsSource = data.DefaultView;
        }
        private void initOverview()
        {
            MySqlCommand cmd = new MySqlCommand("select * from kategori where status = 0", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cb_kategori.Items.Add(reader.GetString(1));
            }
            terisi.IsChecked = false;
            kosong.IsChecked = false;
            kriteria.Items.Add("Titip");
            kriteria.Items.Add("Ambil");
            waktu.Items.Add("Terbaru");
            waktu.Items.Add("Terlama");
            cmd = new MySqlCommand("select count(*) from kotak where terpakai = 1 and status = 0", conn);
            conn.Close();
            conn.Open();
            kotak_terisi.Text = "Kotak Terisi : " + cmd.ExecuteScalar().ToString();
            conn.Close();
            cmd = new MySqlCommand("select count(*) from kotak where terpakai = 0 and status = 0", conn);
            conn.Close();
            conn.Open();
            kotak_kosong.Text = "Kotak Kosong : " + cmd.ExecuteScalar().ToString();
            conn.Close();
        }

        private void filter_kategori_Click(object sender, RoutedEventArgs e)
        {
            if (cb_kategori.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pilih Filter Kategori terlebih dahulu!");
                return;
            }
            MySqlCommand cmd = new MySqlCommand("select * from kategori where nama = ?nama", conn);
            cmd.Parameters.AddWithValue("?nama", cb_kategori.SelectedItem.ToString());
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            int id_kategori = -1;
            while (reader.Read())
            {
                id_kategori = reader.GetInt32(0);
            }
            cmd = new MySqlCommand("select kotak.no_kotak as 'No Kotak', kategori.nama as 'Kategori' from kotak join kategori on kotak.kategori_id = kategori.id where kategori.id = ?id_kategori and kotak.status = 0", conn);
            cmd.Parameters.AddWithValue("?id_kategori", id_kategori);
            conn.Close();
            conn.Open();
            data = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(data);
            dtGrid.ItemsSource = data.DefaultView;
            conn.Close();
        }

        private void reset_filter_Click(object sender, RoutedEventArgs e)
        {
            cb_kategori.SelectedIndex = -1;
            kosong.IsChecked = false;
            terisi.IsChecked = false;
            kriteria.SelectedIndex = -1;
            waktu.SelectedIndex = -1;
            loadDataGrid(tb_pencarian.Text);
        }

        private void filter_kotak_Click(object sender, RoutedEventArgs e)
        {
            if (kosong.IsChecked == false && terisi.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Pilih Filter Kotak terlebih dahulu!");
                return;
            }
            MySqlCommand cmd = new MySqlCommand("select kotak.no_kotak as 'No Kotak', kategori.nama as 'Kategori' from kotak join kategori on kotak.kategori_id = kategori.id where kotak.status = 0 and kotak.terpakai = ?terpakai", conn);
            if (kosong.IsChecked == true) cmd.Parameters.AddWithValue("?terpakai", 0);
            if (terisi.IsChecked == true) cmd.Parameters.AddWithValue("?terpakai", 1);
            conn.Close();
            conn.Open();
            data = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(data);
            dtGrid.ItemsSource = data.DefaultView;
            conn.Close();
        }

        private void filter_tanggal_Click(object sender, RoutedEventArgs e)
        {
            if (kriteria.SelectedIndex == -1 || waktu.SelectedIndex == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pilih Filter Tanggal terlebih dahulu!");
                return;
            }
            if (kriteria.SelectedIndex == 0 && waktu.SelectedIndex == 0)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT p.id as 'ID', k.no_kotak AS 'No Kotak', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', da.alamat_abu AS 'Alamat Abu', da.jenis_kelamin AS 'Jenis Kelamin',  DATE(da.tanggal_lahir) AS 'Tanggal Lahir',  DATE(da.tanggal_wafat) AS 'Tanggal Wafat', DATE(da.tanggal_kremasi) AS 'Tanggal Kremasi', DATE(p.tanggal_titip) AS 'Tanggal Titip', DATE(p.tanggal_ambil) AS 'Tanggal Ambil', da.keterangan AS 'Keterangan', pj1.nama AS 'Nama Penanggung Jawab 1', pj2.nama AS 'Nama Penanggung Jawab 2' FROM penitipan p LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN kotak k ON k.id = p.kotak_id LEFT JOIN penanggung_jawab pj1 ON p.penanggung_jawab_satu_id = pj1.id LEFT JOIN penanggung_jawab pj2 ON p.penanggung_jawab_dua_id = pj2.id where p.tanggal_titip <= ?tanggal_titip and p.tanggal_ambil >= ?tanggal_ambil order by p.tanggal_titip desc", conn);
                cmd.Parameters.AddWithValue("?tanggal_titip", DateTime.Today);
                cmd.Parameters.AddWithValue("?tanggal_ambil", DateTime.Today);
                data = new DataTable();
                conn.Close();
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(data);
                dtGrid.ItemsSource = data.DefaultView;
            }
            else if (kriteria.SelectedIndex == 0 && waktu.SelectedIndex == 1)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT p.id as 'ID', k.no_kotak AS 'No Kotak', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', da.alamat_abu AS 'Alamat Abu', da.jenis_kelamin AS 'Jenis Kelamin',  DATE(da.tanggal_lahir) AS 'Tanggal Lahir',  DATE(da.tanggal_wafat) AS 'Tanggal Wafat', DATE(da.tanggal_kremasi) AS 'Tanggal Kremasi', DATE(p.tanggal_titip) AS 'Tanggal Titip', DATE(p.tanggal_ambil) AS 'Tanggal Ambil', da.keterangan AS 'Keterangan', pj1.nama AS 'Nama Penanggung Jawab 1', pj2.nama AS 'Nama Penanggung Jawab 2' FROM penitipan p LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN kotak k ON k.id = p.kotak_id LEFT JOIN penanggung_jawab pj1 ON p.penanggung_jawab_satu_id = pj1.id LEFT JOIN penanggung_jawab pj2 ON p.penanggung_jawab_dua_id = pj2.id where p.tanggal_titip <= ?tanggal_titip and p.tanggal_ambil >= ?tanggal_ambil order by p.tanggal_titip asc", conn);
                cmd.Parameters.AddWithValue("?tanggal_titip", DateTime.Today);
                cmd.Parameters.AddWithValue("?tanggal_ambil", DateTime.Today);
                data = new DataTable();
                conn.Close();
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(data);
                dtGrid.ItemsSource = data.DefaultView;
            }
            else if (kriteria.SelectedIndex == 1 && waktu.SelectedIndex == 0)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT p.id as 'ID', k.no_kotak AS 'No Kotak', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', da.alamat_abu AS 'Alamat Abu', da.jenis_kelamin AS 'Jenis Kelamin',  DATE(da.tanggal_lahir) AS 'Tanggal Lahir',  DATE(da.tanggal_wafat) AS 'Tanggal Wafat', DATE(da.tanggal_kremasi) AS 'Tanggal Kremasi', DATE(p.tanggal_titip) AS 'Tanggal Titip', DATE(p.tanggal_ambil) AS 'Tanggal Ambil', da.keterangan AS 'Keterangan', pj1.nama AS 'Nama Penanggung Jawab 1', pj2.nama AS 'Nama Penanggung Jawab 2' FROM penitipan p LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN kotak k ON k.id = p.kotak_id LEFT JOIN penanggung_jawab pj1 ON p.penanggung_jawab_satu_id = pj1.id LEFT JOIN penanggung_jawab pj2 ON p.penanggung_jawab_dua_id = pj2.id where p.tanggal_titip <= ?tanggal_titip and p.tanggal_ambil >= ?tanggal_ambil order by p.tanggal_ambil asc", conn);
                cmd.Parameters.AddWithValue("?tanggal_titip", DateTime.Today);
                cmd.Parameters.AddWithValue("?tanggal_ambil", DateTime.Today);
                data = new DataTable();
                conn.Close();
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(data);
                dtGrid.ItemsSource = data.DefaultView;
            }
            else if (kriteria.SelectedIndex == 1 && waktu.SelectedIndex == 1)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT p.id as 'ID', k.no_kotak AS 'No Kotak', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', da.alamat_abu AS 'Alamat Abu', da.jenis_kelamin AS 'Jenis Kelamin',  DATE(da.tanggal_lahir) AS 'Tanggal Lahir',  DATE(da.tanggal_wafat) AS 'Tanggal Wafat', DATE(da.tanggal_kremasi) AS 'Tanggal Kremasi', DATE(p.tanggal_titip) AS 'Tanggal Titip', DATE(p.tanggal_ambil) AS 'Tanggal Ambil', da.keterangan AS 'Keterangan', pj1.nama AS 'Nama Penanggung Jawab 1', pj2.nama AS 'Nama Penanggung Jawab 2' FROM penitipan p LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN kotak k ON k.id = p.kotak_id LEFT JOIN penanggung_jawab pj1 ON p.penanggung_jawab_satu_id = pj1.id LEFT JOIN penanggung_jawab pj2 ON p.penanggung_jawab_dua_id = pj2.id where p.tanggal_titip <= ?tanggal_titip and p.tanggal_ambil >= ?tanggal_ambil order by p.tanggal_ambil desc", conn);
                cmd.Parameters.AddWithValue("?tanggal_titip", DateTime.Today);
                cmd.Parameters.AddWithValue("?tanggal_ambil", DateTime.Today);
                data = new DataTable();
                conn.Close();
                conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(data);
                dtGrid.ItemsSource = data.DefaultView;
            }
        }

        private void tb_pencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadDataGrid(tb_pencarian.Text);
        }
    }
}
