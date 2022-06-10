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
using MySql.Data.MySqlClient;

namespace Aplikasi_Penitipan_Abu.Transaksi.PerpanjanganSewa
{
    /// <summary>
    /// Interaction logic for PerpanjanganSewa_Add.xaml
    /// </summary>
    public partial class PerpanjanganSewa_Add : Page
    {
        MySqlConnection conn;
        Registrasi registrasi;
        int selectedId = -1;
        bool perlu_jaminan = false;
        DateTime awal;
        string penanggung_jawab;
        public PerpanjanganSewa_Add()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }
        private void perhitungan_harga()
        {
            if (tanggal_akhir.SelectedDate < awal)
            {
                System.Windows.Forms.MessageBox.Show("Tanggal Ambil kurang dari Tanggal Titip");
                tanggal_akhir.SelectedDate = awal;
                return;
            }
            //perhitungan harga
            if (registrasi != null && registrasi.harga_kotak != -1 && tanggal_akhir.SelectedDate != null)
            {
                perlu_jaminan = false;
                int harga = registrasi.harga_kotak;
                TimeSpan timeSpan = (TimeSpan)(tanggal_akhir.SelectedDate - awal);
                int countDays = (int)timeSpan.TotalDays;
                int totalMonth = countDays / 30;
                if ((countDays / 30) % 30 >= 0)
                {
                    totalMonth += 1;
                }
                int harga_total_sewa = harga * totalMonth;
                harga_sewa.Text = "Rp." + harga_total_sewa.ToString();
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

        private void tanggal_akhir_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            perhitungan_harga();
        }

        private void btn_cari_Click(object sender, RoutedEventArgs e)
        {
            PencarianRegistrasi pencarian = new PencarianRegistrasi();
            pencarian.ShowDialog();
            this.selectedId = pencarian.selectedId;
            if (selectedId == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
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
            MySqlCommand cmd = new MySqlCommand("select count(*)+1 from pembayaran_sewa where kode_pembayaran like ?tanggal", conn);
            cmd.Parameters.AddWithValue("?tanggal", "%" + tanggal + "%");
            conn.Close();
            conn.Open();
            int jumlah = Int32.Parse(cmd.ExecuteScalar().ToString());
            conn.Close();
            String kode = "KWI-" + tanggal + "-" + jumlah.ToString().PadLeft(5, '0');
            no_kwitansi.Text = kode;
            cmd = new MySqlCommand("select * from penitipan p left join data_abu da on p.data_abu_id = da.id left join penanggung_jawab pj on p.penanggung_jawab_satu_id = pj.id where p.id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", selectedId);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            registrasi = new Registrasi();
            registrasi.idRegistrasi = selectedId;
            while (reader.Read())
            {
                registrasi.id_kotak = reader.GetInt32(4);
                registrasi.id_abu = reader.GetInt32(5);
                registrasi.nama_abu = reader.GetString(11);
                awal = reader.GetDateTime(3);
                penanggung_jawab = reader.GetString(20);
            }
            reader.Close();
            conn.Close();
            cmd = new MySqlCommand("select kode_penitipan from penitipan where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", registrasi.idRegistrasi);
            conn.Close();
            conn.Open();
            no_registrasi.Text = cmd.ExecuteScalar().ToString();
            conn.Close();
            if (registrasi.id_kotak == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            tanggal_awal.Text = awal.ToString("dd/MM/yyyy");
            cmd = new MySqlCommand("SELECT * FROM kotak LEFT JOIN kategori ON kotak.kategori_id = kategori.id WHERE kotak.id = ?idKotak", conn);
            cmd.Parameters.AddWithValue("?idKotak", registrasi.id_kotak);
            conn.Close();
            conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                registrasi.no_kotak = reader.GetString(2);
                registrasi.harga_kotak = reader.GetInt32(8);
            }
            reader.Close();
            conn.Close();
            no_kotak.Text = registrasi.no_kotak;
            nama_abu.Text = registrasi.nama_abu;
            conn.Close();
            perhitungan_harga();
        }

        private void btn_cetak_Click(object sender, RoutedEventArgs e)
        {
            if (registrasi.idRegistrasi == -1)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan pencarian data terlebih dahulu!");
                return;
            }
            if (tanggal_akhir.SelectedDate.ToString() == "" || harga_sewa.Text == "Rp.-")
            {
                System.Windows.Forms.MessageBox.Show("Data belum lengkap!");
                return;
            }
            string jangka_waktu = awal.ToString("dd/MM/yyyy") + " - " + tanggal_akhir.SelectedDate.Value.ToString("dd/MM/yyyy");
            TandaTerimaPerpanjanganSewa tandaTerima = new TandaTerimaPerpanjanganSewa(new tandaTerimaPerpanjanganSewaData(no_kwitansi.Text, no_registrasi.Text, DateTime.Now.ToString("dd/MM/yyyy"), no_kotak.Text, nama_abu.Text, penanggung_jawab, jangka_waktu, Int32.Parse(harga_sewa.Text.Split('.')[1])));
            tandaTerima.Show();
            System.Windows.Forms.MessageBox.Show("Jangan lupa untuk Melakukan Penyimpanan Data!");
            
        }

        private void btn_simpan_Click(object sender, RoutedEventArgs e)
        {
            if (cb_centang.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Centang sudah menerima pembayaran", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            try
            {
                int harga_total_sewa = Int32.Parse(harga_sewa.Text.Split('.')[1]);
                MySqlCommand cmd = new MySqlCommand("insert into pembayaran_sewa (id_penitipan,id_kotak,harga_kotak,harga_total_sewa,tanggal_awal,tanggal_akhir, kode_pembayaran) values(?id_penitipan,?id_kotak,?harga_kotak,?harga_total_sewa,?tanggal_awal,?tanggal_akhir,?kode)", conn);
                cmd.Parameters.AddWithValue("?id_penitipan", registrasi.idRegistrasi);
                cmd.Parameters.AddWithValue("?id_kotak", registrasi.id_kotak);
                cmd.Parameters.AddWithValue("?harga_kotak", registrasi.harga_kotak);
                cmd.Parameters.AddWithValue("?harga_total_sewa", harga_total_sewa);
                cmd.Parameters.AddWithValue("?tanggal_awal", awal);
                cmd.Parameters.AddWithValue("?tanggal_akhir", tanggal_akhir.SelectedDate);
                cmd.Parameters.AddWithValue("?kode", no_kwitansi.Text);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                if (perlu_jaminan)
                {
                    cmd = new MySqlCommand("select count(*) from jaminan where id_penitipan = ?id_penitipan", conn);
                    cmd.Parameters.AddWithValue("?id_penitipan", registrasi.idRegistrasi);
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
                        cmd.Parameters.AddWithValue("?id_penitipan", registrasi.idRegistrasi);
                        conn.Close();
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        PembayaranJaminanPerpanjangan pj = new PembayaranJaminanPerpanjangan(registrasi.idRegistrasi);
                        pj.ShowDialog();
                        if (!pj.IsSimpan)
                        {
                            System.Windows.Forms.MessageBox.Show("Gagal Melakukan Penyimpanan Pada Pembayaran Jaminan", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                conn.Close();
                cmd = new MySqlCommand("update kotak set terpakai = 1, booking = 0 where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", registrasi.id_kotak);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                cmd = new MySqlCommand("update penitipan set tanggal_ambil = ?tanggal_ambil where id = ?id", conn);
                cmd.Parameters.AddWithValue("?tanggal_ambil", tanggal_akhir.SelectedDate);
                cmd.Parameters.AddWithValue("?id", registrasi.idRegistrasi);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Perpanjangan Sewa", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                //reset tampilan
                registrasi = new Registrasi();
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Pastikan semua input sudah benar", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            no_registrasi.Text = "-";
            no_kotak.Text = "-";
            nama_abu.Text = "-";
            harga_sewa.Text = "Rp.";
            tanggal_awal.Text = "-";
            no_kwitansi.Text = "-";
            tanggal_akhir.SelectedDate = null;
            cb_centang.IsChecked = false;
        }
    }
    public class tandaTerimaPerpanjanganSewaData
    {
        public string no_kwitansi;
        public string no_registrasi;
        public string tanggal_pembayaran;
        public string jangka_waktu;
        public string no_kotak;
        public string nama_abu;
        public string penanggung_jawab;
        public int uang;
        public tandaTerimaPerpanjanganSewaData(string noKwitansi, string noRegistrasi, string tanggalPembayaran, string noKotak, string namaAbu, string namaPenanggungJawab, string jangka_waktu, int uang)
        {
            no_kwitansi = noKwitansi;
            no_registrasi = noRegistrasi;
            tanggal_pembayaran = tanggalPembayaran;
            this.jangka_waktu = jangka_waktu;
            no_kotak = noKotak;
            nama_abu = namaAbu;
            penanggung_jawab = namaPenanggungJawab;
            this.uang = uang;
        }
    }
}
