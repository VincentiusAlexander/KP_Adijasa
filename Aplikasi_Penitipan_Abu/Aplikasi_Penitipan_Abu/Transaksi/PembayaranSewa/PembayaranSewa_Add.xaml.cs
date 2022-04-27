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
            command.CommandText = "select max(id) from pembayaran_sewa";
            no_kwitansi.Text = ((int)command.ExecuteScalar() + 1).ToString();

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
                registrasi.harga_kotak = reader.GetInt32(7);
            }

            reader.Close();
            registrasi.idRegistrasi = selectedId;
            no_kotak.Text = registrasi.no_kotak;
            no_registrasi.Text = registrasi.idRegistrasi.ToString();
            nama_abu.Text = registrasi.nama_abu;

            conn.Close();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datepickerAwal.SelectedDate != null && datepickerAkhir.SelectedDate != null)
            {
                //perhitungan harga
                if(registrasi != null && registrasi.harga_kotak != -1) {
                    int harga = registrasi.harga_kotak;
                    if (datepickerAkhir.SelectedDate < datepickerAwal.SelectedDate)
                    {
                        System.Windows.Forms.MessageBox.Show("tanggal akhir lebih awal daripada tanggal awal");
                        datepickerAkhir.SelectedDate = datepickerAwal.SelectedDate;
                        return;
                    }
                    TimeSpan timeSpan = (TimeSpan)(datepickerAkhir.SelectedDate - datepickerAwal.SelectedDate);
                    int countDays = (int) timeSpan.TotalDays;
                    int totalMonth = countDays / 30;
                    if((countDays/30)%30 >= 0)
                    {
                        totalMonth += 1;
                    }
                    int harga_total_sewa = harga * totalMonth;
                    harga_sewa.Text = "Rp."+harga_total_sewa.ToString();
                }
            }
        }

        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {
            if (centang_bila_pembayaran.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Centang sudah menerima pembayaran", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

            int harga_total_sewa = Int32.Parse(harga_sewa.Text.Split('.')[1]);
            DateTime tanggal_awal = (DateTime)datepickerAwal.SelectedDate;
            DateTime tanggal_akhir = (DateTime)datepickerAkhir.SelectedDate;

            MySqlCommand cmd = new MySqlCommand("insert into pembayaran_sewa (id_penitipan,id_kotak,harga_kotak,harga_total_sewa,tanggal_awal,tanggal_akhir) values(?id_penitipan,?id_kotak,?harga_kotak,?harga_total_sewa,?tanggal_awal,?tanggal_akhir)", conn);
            cmd.Parameters.AddWithValue("?id_penitipan", registrasi.idRegistrasi);
            cmd.Parameters.AddWithValue("?id_kotak", registrasi.id_kotak);
            cmd.Parameters.AddWithValue("?harga_kotak", registrasi.harga_kotak);
            cmd.Parameters.AddWithValue("?harga_total_sewa", harga_total_sewa);
            cmd.Parameters.AddWithValue("?tanggal_awal", tanggal_awal.ToString("yyyy-MM-dd HH:mm"));
            cmd.Parameters.AddWithValue("?tanggal_akhir", tanggal_akhir.ToString("yyyy-MM-dd HH:mm"));
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
            datepickerAkhir.SelectedDate = null;
            datepickerAwal.SelectedDate = null;
        }

        private void cetak_tanda_terima_pembayaran_abu_Click(object sender, RoutedEventArgs e)
        {
            TandaTerimaPembayaranSewa tandaTerima = new TandaTerimaPembayaranSewa();
            tandaTerima.Show();
        }
    }
}
