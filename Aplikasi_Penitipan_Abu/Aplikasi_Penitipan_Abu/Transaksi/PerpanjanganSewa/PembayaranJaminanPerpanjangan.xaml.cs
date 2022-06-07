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
using System.Windows.Shapes;
using Aplikasi_Penitipan_Abu.Transaksi.PembayaranJaminan;
using MySql.Data.MySqlClient;

namespace Aplikasi_Penitipan_Abu.Transaksi.PerpanjanganSewa
{
    /// <summary>
    /// Interaction logic for PembayaranJaminanPerpanjangan.xaml
    /// </summary>
    public partial class PembayaranJaminanPerpanjangan : Window
    {
        MySqlConnection conn;
        int selectedId = -1;
        int id_penitipan = -1;
        bool isSimpan = false;

        public bool IsSimpan { get => isSimpan; set => isSimpan = value; }

        public PembayaranJaminanPerpanjangan(int id_penitipan)
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            this.id_penitipan = id_penitipan;
            cari_jaminan();
            System.Windows.Forms.MessageBox.Show("Diperlukan Pembayaran Jaminan Untuk Penyimpanan Jangka Panjang", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        private void cari_jaminan()
        {
            int data_abu_id = -1;
            int penanggung_jawab_satu_id = -1;
            this.selectedId = this.id_penitipan;
            if (selectedId == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            no_kwitansi_jaminan_txt.Text = selectedId.ToString();
            MySqlCommand cmd = new MySqlCommand("select * from jaminan where id_penitipan = ?id", conn);
            cmd.Parameters.AddWithValue("?id", selectedId);
            conn.Close();
            conn.Open();
            int id_penitipan = -1;
            int total_jaminan = -1;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_penitipan = reader.GetInt32(1);
                total_jaminan = reader.GetInt32(2);
            }
            no_registrasi_txt.Text = id_penitipan.ToString();
            uang_jaminan_txt.Text = "Rp. " + total_jaminan.ToString();
            reader.Close();
            cmd.CommandText = "select * from penitipan where id = ?id_penitipan";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("?id_penitipan", id_penitipan);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data_abu_id = reader.GetInt32(5);
                penanggung_jawab_satu_id = reader.GetInt32(6);
            }
            reader.Close();
            cmd.CommandText = "select * from data_abu where id = ?data_abu_id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("?data_abu_id", data_abu_id);
            string nama_abu = "";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nama_abu = reader.GetString(1);
            }
            nama_abu_txt.Text = nama_abu;
            reader.Close();
            cmd.CommandText = "select * from penanggung_jawab where id = ?id";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("?id", penanggung_jawab_satu_id);
            string nama_penanggung_jawab = "";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nama_penanggung_jawab = reader.GetString(1);
            }
            nama_penanggung_jawab_txt.Text = nama_penanggung_jawab;
            reader.Close();
        }

        private void btn_simpan_Click(object sender, RoutedEventArgs e)
        {
            if (check_box_pembayaran_telah_diterima.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Centang Pembayaran Sudah Diterima", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            if (selectedId == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("update jaminan set status = 1 where id = ?id", conn);
            conn.Close();
            conn.Open();
            cmd.Parameters.AddWithValue("?id", selectedId);
            cmd.ExecuteNonQuery();
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Berhasil melakukan pembayaran uang jaminan", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            isSimpan = true;
        }

        private void btnCetak_Click(object sender, RoutedEventArgs e)
        {
            if (!isSimpan)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan Penyimpanan Terlebih Dahulu", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            TandaTerimaPembayaranJaminanFix a = new TandaTerimaPembayaranJaminanFix(new tandaTerimaPembayaranJaminanData(no_kwitansi_jaminan_txt.Text, no_registrasi_txt.Text, DateTime.Now.ToString("dd/MM/yyyy"), nama_penanggung_jawab_txt.Text, nama_abu_txt.Text, uang_jaminan_txt.Text));
            a.ShowDialog();
        }

        private void btn_selesai_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
