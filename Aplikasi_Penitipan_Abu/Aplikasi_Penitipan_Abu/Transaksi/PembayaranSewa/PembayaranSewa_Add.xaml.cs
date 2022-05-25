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
        bool perlu_jaminan = false;
        DateTime awal, akhir;
        string penanggung_jawab;
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
            MySqlCommand command = new MySqlCommand("select count(*) from pembayaran_sewa where id_penitipan = ?id", conn);
            command.Parameters.AddWithValue("?id", selectedId);
            conn.Close();
            conn.Open();
            int count = Int32.Parse(command.ExecuteScalar().ToString());

            if(count > 0)
            {
                System.Windows.Forms.MessageBox.Show("Pembayaran Sewa untuk id ini telah dilakukan sebelumnya !", "Pembayaran Sewa", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            conn.Close();
            command.Parameters.Clear();
            conn.Open();
            command.CommandText = "select count(*) from pembayaran_sewa";
            int max = Int32.Parse(command.ExecuteScalar().ToString()) + 1;
            no_kwitansi.Text = max.ToString();

            MySqlCommand cmd = new MySqlCommand("select * from penitipan p left join data_abu da on p.data_abu_id = da.id left join penanggung_jawab pj on p.penanggung_jawab_satu_id = pj.id where p.id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", selectedId);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            registrasi = new Registrasi();
            while (reader.Read())
            {
                registrasi.id_kotak = reader.GetInt32(4);
                registrasi.id_abu = reader.GetInt32(5);
                registrasi.nama_abu = reader.GetString(10);
                awal = reader.GetDateTime(2);
                akhir = reader.GetDateTime(3);
                penanggung_jawab = reader.GetString(19);
            }
            reader.Close();
            if (registrasi.id_kotak == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            tanggalAwal.Text = awal.ToString("dd/M/yyyy");
            tanggalAkhir.Text = akhir.ToString("dd/M/yyyy");
            cmd.CommandText = "SELECT * FROM kotak LEFT JOIN kategori ON kotak.kategori_id = kategori.id WHERE kotak.id = ?idKotak";
            cmd.Parameters.AddWithValue("?idKotak", registrasi.id_kotak);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                registrasi.no_kotak = reader.GetString(2);
                registrasi.harga_kotak = reader.GetInt32(7);
            }

            reader.Close();
            registrasi.idRegistrasi = selectedId;
            no_kotak.Text = registrasi.no_kotak;
            no_registrasi.Text = registrasi.idRegistrasi.ToString();
            nama_abu.Text = registrasi.nama_abu;
            //perhitungan harga
            if (registrasi != null && registrasi.harga_kotak != -1)
            {
                perlu_jaminan = false;
                int harga = registrasi.harga_kotak;
                TimeSpan timeSpan = (TimeSpan)(akhir - awal);
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
            }
            conn.Close();
        }

        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {
            if (centang_bila_pembayaran.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Centang sudah menerima pembayaran", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            int harga_total_sewa = Int32.Parse(harga_sewa.Text.Split('.')[1]);

            MySqlCommand cmd = new MySqlCommand("insert into pembayaran_sewa (id_penitipan,id_kotak,harga_kotak,harga_total_sewa,tanggal_awal,tanggal_akhir) values(?id_penitipan,?id_kotak,?harga_kotak,?harga_total_sewa,?tanggal_awal,?tanggal_akhir)", conn);
            cmd.Parameters.AddWithValue("?id_penitipan", registrasi.idRegistrasi);
            cmd.Parameters.AddWithValue("?id_kotak", registrasi.id_kotak);
            cmd.Parameters.AddWithValue("?harga_kotak", registrasi.harga_kotak);
            cmd.Parameters.AddWithValue("?harga_total_sewa", harga_total_sewa);
            cmd.Parameters.AddWithValue("?tanggal_awal", awal);
            cmd.Parameters.AddWithValue("?tanggal_akhir", akhir);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            if (perlu_jaminan)
            {
                cmd.CommandText = "insert into jaminan values(0,?id_penitipan,1000000,0)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("?id_penitipan", registrasi.idRegistrasi);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            cmd = new MySqlCommand("update kotak set terpakai = 1, booking = 0 where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", registrasi.id_kotak);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Pembayaran Sewa", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            //reset tampilan
            registrasi = new Registrasi();
            no_registrasi.Text = "-";
            no_kotak.Text = "-";
            nama_abu.Text = "-";
            harga_sewa.Text = "Rp.";
            tanggalAkhir.Text = "-";
            tanggalAwal.Text = "-";
        }

        private void cetak_tanda_terima_pembayaran_abu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TandaTerimaPembayaranSewa tandaTerima = new TandaTerimaPembayaranSewa(new tandaTerimaPembayaranSewaData(Int32.Parse(no_kwitansi.Text), Int32.Parse(no_registrasi.Text), DateTime.Now.ToString("dd/MM/yyyy"), no_kotak.Text, nama_abu.Text, penanggung_jawab));
                tandaTerima.Show();
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan Pencarian Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
        }
    }
    public class tandaTerimaPembayaranSewaData
    {
        public int no_kwitansi;
        public int no_registrasi;
        public string tanggal_pembayaran;
        public string no_kotak;
        public string nama_abu;
        public string penanggung_jawab;
        public tandaTerimaPembayaranSewaData(int noKwitansi, int noRegistrasi, string tanggalPembayaran, string noKotak, string namaAbu, string namaPenanggungJawab)
        {
            no_kwitansi = noKwitansi;
            no_registrasi = noRegistrasi;
            tanggal_pembayaran = tanggalPembayaran;
            no_kotak = noKotak;
            nama_abu = namaAbu;
            penanggung_jawab = namaPenanggungJawab;
        }
    }

}
