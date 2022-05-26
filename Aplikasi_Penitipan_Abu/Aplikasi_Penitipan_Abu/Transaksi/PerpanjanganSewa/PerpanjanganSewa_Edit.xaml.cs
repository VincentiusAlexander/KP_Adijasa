using System;
using System.Collections;
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
using MySql.Data.MySqlClient;

namespace Aplikasi_Penitipan_Abu.Transaksi.PerpanjanganSewa
{
    /// <summary>
    /// Interaction logic for PerpanjanganSewa_Edit.xaml
    /// </summary>
    public partial class PerpanjanganSewa_Edit : Page
    {
        MySqlConnection conn;
        int idPembayaranSewa = -1;
        int id_kotak;
        Registrasi registrasi;
        int id_penitipan;
        int harga_kotak = -1;
        int harga_total_sewa;
        DateTime tanggal_awal;
        string penanggung_jawab;
        ArrayList list = new ArrayList();
        public PerpanjanganSewa_Edit()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        public void loadComboBox()
        {
            MySqlCommand command = new MySqlCommand("select * from kotak where terpakai = 0 or id = ?id", conn);
            command.Parameters.AddWithValue("?id", id_kotak);
            conn.Close();
            conn.Open();

            MySqlDataReader reader_kotak = command.ExecuteReader();
            while (reader_kotak.Read())
            {
                list.Add(new Kotak(reader_kotak.GetInt32(0), reader_kotak.GetString(2)));
            }
            no_kotak.ItemsSource = list;
            no_kotak.DisplayMemberPath = "no_kotak";
            no_kotak.SelectedValuePath = "id";
        }

        private void btn_cari_Click(object sender, RoutedEventArgs e)
        {
            PencarianPembayaranSewaPerpanjangan pencarian = new PencarianPembayaranSewaPerpanjangan();
            pencarian.ShowDialog();
            idPembayaranSewa = pencarian.selectedId;
            if (idPembayaranSewa == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            no_kwitansi.Text = idPembayaranSewa.ToString();
            MySqlCommand cmd = new MySqlCommand("select * from pembayaran_sewa where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", idPembayaranSewa);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_penitipan = reader.GetInt32(1);
                id_kotak = reader.GetInt32(2);
                harga_kotak = reader.GetInt32(3);
                harga_total_sewa = reader.GetInt32(4);
                tanggal_awal = reader.GetDateTime(5);
            }
            loadComboBox();
            no_registrasi.Text = id_penitipan.ToString();
            for (int i = 0; i < list.Count; i++)
            {
                Kotak item = (Kotak)list[i];
                if (id_kotak == item.id)
                {
                    no_kotak.SelectedIndex = i;
                }
            }
            reader.Close();
            conn.Close();
            conn.Open();
            cmd.Parameters.Clear();
            cmd.CommandText = "select da.nama_abu from penitipan p left join data_abu da on p.data_abu_id = da.id where p.id = ?id";
            cmd.Parameters.AddWithValue("?id", id_penitipan);
            nama_abu.Text = cmd.ExecuteScalar().ToString();
            conn.Close();
            tanggal_simpan_awal.Text = tanggal_awal.ToString("dd/M/yyyy");
            //harga_total_sewa_txt.Text = "Rp." + harga_total_sewa.ToString();
        }

        private void tanggal_simpan_akhir_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tanggal_simpan_akhir.SelectedDate < tanggal_awal)
            {
                System.Windows.Forms.MessageBox.Show("Tanggal Ambil kurang dari Tanggal Titip");
                tanggal_simpan_akhir.SelectedDate = tanggal_awal;
                return;
            }
            perhitungan_harga();
        }

        private void perhitungan_harga()
        {
            if (tanggal_simpan_akhir.SelectedDate != null)
            {
                int harga = harga_kotak;
                TimeSpan timeSpan = (TimeSpan)(tanggal_simpan_akhir.SelectedDate - tanggal_awal);
                int countDays = (int)timeSpan.TotalDays;
                int totalMonth = countDays / 30;
                if ((countDays / 30) % 30 >= 0)
                {
                    totalMonth += 1;
                }
                int harga_total_sewa = harga * totalMonth;
                harga_total_sewa_txt.Text = "Rp." + harga_total_sewa.ToString();
            }
        }

        private void btn_cetak_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TandaTerimaPerpanjanganSewa tandaTerima = new TandaTerimaPerpanjanganSewa(new tandaTerimaPerpanjanganSewaData(Int32.Parse(no_kwitansi.Text), Int32.Parse(no_registrasi.Text), DateTime.Now.ToString("dd/MM/yyyy"), no_kotak.Text, nama_abu.Text, penanggung_jawab));
                tandaTerima.Show();
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan Pencarian Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
        }
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            if (cb_centang.IsChecked != true)
            {
                System.Windows.Forms.MessageBox.Show("Centang Pembayaran Telah Diterima", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("Update pembayaran_sewa set id_kotak = ?id_kotak, harga_kotak = ?harga_kotak, harga_total_sewa = ?harga_total_sewa, tanggal_awal = ?tanggal_awal, tanggal_akhir = ?tanggal_akhir where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id_kotak", id_kotak);
            cmd.Parameters.AddWithValue("?harga_kotak", harga_kotak);
            cmd.Parameters.AddWithValue("?harga_total_sewa", harga_total_sewa);
            cmd.Parameters.AddWithValue("?tanggal_awal", tanggal_awal);
            cmd.Parameters.AddWithValue("?tanggal_akhir", ((DateTime)tanggal_simpan_akhir.SelectedDate).ToString("yyyy-MM-dd HH:mm"));
            cmd.Parameters.AddWithValue("?id", idPembayaranSewa);
            conn.Close();
            conn.Open();
            cmd.ExecuteScalar();
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Edit Berhasil", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            resetTampilan();
        }

        private void resetTampilan()
        {
            no_kwitansi.Text = "-";
            no_registrasi.Text = "-";
            no_kotak.SelectedIndex = -1;
            nama_abu.Text = "-";
            tanggal_simpan_awal.Text = "-";
            tanggal_simpan_akhir.SelectedDate = null;
            harga_total_sewa_txt.Text = "Rp.-";
            idPembayaranSewa = -1;
            id_kotak = -1;
            id_penitipan = -1;
            harga_kotak = -1;
            harga_total_sewa = -1;
            tanggal_awal = new DateTime();
            cb_centang.IsChecked = false;
        }
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd = new MySqlCommand("Update pembayaran_sewa set status = 1 where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", idPembayaranSewa);
            conn.Close();
            conn.Open();
            cmd.ExecuteScalar();
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Penghapusan Berhasil", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            resetTampilan();
        }

        private void no_kotak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Kotak kotak = (Kotak)no_kotak.SelectedItem;
            if (kotak != null)
            {
                MySqlCommand cmd = new MySqlCommand("select k.id, kt.harga from kotak k left join kategori kt on k.kategori_id = kt.id where k.id = ?id", conn);
                conn.Close();
                conn.Open();
                cmd.Parameters.AddWithValue("?id", kotak.id);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id_kotak = reader.GetInt32(0);
                    harga_kotak = reader.GetInt32(1);
                }
                conn.Close();
                perhitungan_harga();
            }
        }
    }
    public class Kotak
    {
        public int id { get; set; }
        public string no_kotak { get; set; }
        public Kotak(int id, string no_kotak)
        {
            this.id = id;
            this.no_kotak = no_kotak;
        }
    }
}
