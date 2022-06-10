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
        bool perlu_jaminan = false;
        int idPembayaranSewa = -1;
        int id_kotak = -1;
        int id_penitipan = -1;
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
            MySqlCommand command = new MySqlCommand("select * from kotak", conn);
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
            int month = DateTime.Now.Month;
            String bulan = "";
            if (month < 10)
            {
                bulan += "0" + month;
            }
            else
            {
                bulan += month + "";
            }
            String tanggal = DateTime.Now.Year.ToString().Substring(2, 2) + bulan;
            cmd = new MySqlCommand("select count(*)+1 from pembayaran_sewa where kode_pembayaran like ?tanggal", conn);
            cmd.Parameters.AddWithValue("?tanggal", "%" + tanggal + "%");
            conn.Close();
            conn.Open();
            int jumlah = Int32.Parse(cmd.ExecuteScalar().ToString());
            conn.Close();
            String kode = "KWI-" + tanggal + "-" + jumlah.ToString().PadLeft(5, '0');
            no_kwitansi.Text = kode;
            cmd = new MySqlCommand("select kode_penitipan from penitipan where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_penitipan);
            conn.Close();
            conn.Open();
            no_registrasi.Text = cmd.ExecuteScalar().ToString();
            conn.Close();
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
            cmd = new MySqlCommand("select da.nama_abu, pj.nama from penitipan p left join data_abu da on p.data_abu_id = da.id join penanggung_jawab pj on pj.id = p.penanggung_jawab_satu_id where p.id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_penitipan);
            conn.Close();
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nama_abu.Text = reader.GetString(0);
                penanggung_jawab = reader.GetString(1);
            }
            reader.Close();
            conn.Close();
            tanggal_simpan_awal.Text = tanggal_awal.ToString("dd/M/yyyy");
            perhitungan_harga();
        }

        private void tanggal_simpan_akhir_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            perhitungan_harga();
        }

        private void perhitungan_harga()
        {
            if (tanggal_simpan_akhir.SelectedDate < tanggal_awal)
            {
                System.Windows.Forms.MessageBox.Show("Tanggal Ambil kurang dari Tanggal Titip");
                tanggal_simpan_akhir.SelectedDate = tanggal_awal;
                return;
            }
            //perhitungan harga
            if (tanggal_simpan_akhir.SelectedDate != null)
            {
                perlu_jaminan = false;
                MySqlCommand cmd = new MySqlCommand("select ka.harga from kotak ko join kategori ka on ka.id = ko.kategori_id where ko.id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", id_kotak);
                conn.Close();
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                int harga = -1;
                while (reader.Read())
                {
                    harga = reader.GetInt32(0);
                }
                reader.Close();
                conn.Close();
                TimeSpan timeSpan = (TimeSpan)(tanggal_simpan_akhir.SelectedDate - tanggal_awal);
                int countDays = (int)timeSpan.TotalDays;
                int totalMonth = countDays / 30;
                if ((countDays / 30) % 30 >= 0)
                {
                    totalMonth += 1;
                }
                int harga_total_sewa = harga * totalMonth;
                harga_total_sewa_txt.Text = "Rp." + harga_total_sewa.ToString();
                if (totalMonth >= 3)
                {
                    perlu_jaminan = true;
                }
                else
                {
                    perlu_jaminan = false;
                }
            }
        }

        private void btn_cetak_Click(object sender, RoutedEventArgs e)
        {
            if (id_penitipan == -1 || id_penitipan == 0)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan pencarian data terlebih dahulu!");
                return;
            }
            if (tanggal_simpan_akhir.SelectedDate.ToString() == "" || harga_total_sewa_txt.Text == "Rp.-")
            {
                System.Windows.Forms.MessageBox.Show("Data belum lengkap!");
                return;
            }
            string jangka_waktu = tanggal_awal.ToString("dd/MM/yyyy") + " - " + tanggal_simpan_akhir.SelectedDate.Value.ToString("dd/MM/yyyy");
            TandaTerimaPerpanjanganSewa tandaTerima = new TandaTerimaPerpanjanganSewa(new tandaTerimaPerpanjanganSewaData(no_kwitansi.Text, no_registrasi.Text, DateTime.Now.ToString("dd/MM/yyyy"), no_kotak.Text, nama_abu.Text, penanggung_jawab, jangka_waktu, Int32.Parse(harga_total_sewa_txt.Text.Split('.')[1])));
            tandaTerima.Show();
            System.Windows.Forms.MessageBox.Show("Jangan lupa untuk Melakukan Penyimpanan Data!");
        }
        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            if (cb_centang.IsChecked != true)
            {
                System.Windows.Forms.MessageBox.Show("Centang sudah menerima pembayaran", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("select * from kotak where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_kotak);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(4) == 1 || reader.GetInt32(5) == 1)
                {
                    System.Windows.Forms.MessageBox.Show("Kotak Masih Dipakai/Sudah Dibook!");
                    return;
                }
            }
            reader.Close();
            conn.Close();
            cmd = new MySqlCommand("Update pembayaran_sewa set id_kotak = ?id_kotak, harga_kotak = ?harga_kotak, harga_total_sewa = ?harga_total_sewa, tanggal_awal = ?tanggal_awal, tanggal_akhir = ?tanggal_akhir where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id_kotak", id_kotak);
            cmd.Parameters.AddWithValue("?harga_kotak", harga_kotak);
            cmd.Parameters.AddWithValue("?harga_total_sewa", harga_total_sewa);
            cmd.Parameters.AddWithValue("?tanggal_awal", tanggal_awal);
            cmd.Parameters.AddWithValue("?tanggal_akhir", ((DateTime)tanggal_simpan_akhir.SelectedDate).ToString("yyyy-MM-dd HH:mm"));
            cmd.Parameters.AddWithValue("?id", idPembayaranSewa);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            if (perlu_jaminan)
            {
                cmd = new MySqlCommand("select count(*) from jaminan where id_penitipan = ?id_penitipan", conn);
                cmd.Parameters.AddWithValue("?id_penitipan", id_penitipan);
                conn.Close();
                conn.Open();
                int count = Int32.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
                if (count > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Sudah melakukan pembayaran jaminan!");
                }
                else
                {
                    cmd = new MySqlCommand("insert into jaminan values(0,?id_penitipan,1000000,0,0)", conn);
                    cmd.Parameters.AddWithValue("?id_penitipan", id_penitipan);
                    conn.Close();
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    PembayaranJaminanPerpanjangan pj = new PembayaranJaminanPerpanjangan(id_penitipan);
                    pj.ShowDialog();
                    if (!pj.IsSimpan)
                    {
                        System.Windows.Forms.MessageBox.Show("Gagal Melakukan Penyimpanan Pada Pembayaran Jaminan", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            conn.Close();
            cmd = new MySqlCommand("update penitipan set tanggal_ambil = ?tanggal_ambil where id = ?id", conn);
            cmd.Parameters.AddWithValue("?tanggal_ambil", tanggal_simpan_akhir.SelectedDate);
            cmd.Parameters.AddWithValue("?id", id_penitipan);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
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

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            no_registrasi.Text = "-";
            no_kotak.Text = "-";
            nama_abu.Text = "-";
            harga_total_sewa_txt.Text = "Rp.";
            tanggal_simpan_awal.Text = "-";
            no_kwitansi.Text = "-";
            tanggal_simpan_akhir.SelectedDate = null;
            cb_centang.IsChecked = false;
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
