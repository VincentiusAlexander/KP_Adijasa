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

namespace Aplikasi_Penitipan_Abu.Transaksi.PembayaranJaminan
{
    /// <summary>
    /// Interaction logic for PembayaranJaminan_Add.xaml
    /// </summary>
    public partial class PembayaranJaminan_Add : Page
    {
        MySqlConnection conn;
        int selectedId = -1;
        bool isSaved = false;
        public PembayaranJaminan_Add()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        private void btn_cari_registrasi_Click(object sender, RoutedEventArgs e)
        {
            int data_abu_id = -1;
            int penanggung_jawab_satu_id = -1;
            PencarianPembayaranJaminan pencarian = new PencarianPembayaranJaminan();
            pencarian.ShowDialog();
            this.selectedId = pencarian.selectedId;
            if (selectedId == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("select * from jaminan where id = ?id", conn);
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
                no_kwitansi_jaminan_txt.Text = reader.GetString(5);
            }
            uang_jaminan_txt.Text = "Rp. "+total_jaminan.ToString();
            reader.Close();
            cmd.CommandText = "select * from penitipan where id = ?id_penitipan";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("?id_penitipan", id_penitipan);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data_abu_id = reader.GetInt32(5);
                penanggung_jawab_satu_id = reader.GetInt32(6);
                no_registrasi_txt.Text =reader.GetString(9);
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
            if (isSaved)
            {
                return;
            }
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
            MySqlCommand cmd1 = new MySqlCommand("select * from jaminan where id = ?id", conn);
            conn.Close();
            conn.Open();
            cmd1.Parameters.AddWithValue("?id", selectedId);
            MySqlDataReader reader = cmd1.ExecuteReader();
            bool sudah_pernah_terbayar = false;
            while (reader.Read())
            {
                if (reader.GetInt32(3) == 1)
                {
                    sudah_pernah_terbayar = true;
                }
            }
            reader.Close();
            conn.Close();

            if (sudah_pernah_terbayar)
            {
                System.Windows.Forms.MessageBox.Show("Sudah pernah terbayar sebelumnya", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                isSaved = true;
                return;
            }

            MySqlCommand cmd = new MySqlCommand("update jaminan set status = 1 where id = ?id", conn);
            conn.Close();
            conn.Open();
            cmd.Parameters.AddWithValue("?id", selectedId);
            cmd.ExecuteNonQuery();
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Berhasil melakukan pembayaran uang jaminan", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            isSaved = true;
        }

        private void btnCetak_Click(object sender, RoutedEventArgs e)
        {
            if (!isSaved)
            {
                System.Windows.Forms.MessageBox.Show("Lakukan Simpan Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            TandaTerimaPembayaranJaminanFix a = new TandaTerimaPembayaranJaminanFix(new tandaTerimaPembayaranJaminanData(no_kwitansi_jaminan_txt.Text, no_registrasi_txt.Text, DateTime.Now.ToString("dd/MM/yyyy"), nama_penanggung_jawab_txt.Text, nama_abu_txt.Text, uang_jaminan_txt.Text));
            a.ShowDialog();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            selectedId = -1;
            nama_abu_txt.Text = "-";
            nama_penanggung_jawab_txt.Text = "-";
            no_kwitansi_jaminan_txt.Text = "-";
            no_registrasi_txt.Text = "-";
            uang_jaminan_txt.Text = "Rp.";
            check_box_pembayaran_telah_diterima.IsChecked = false;
            isSaved = false;
        }
    }
    public class tandaTerimaPembayaranJaminanData
    {
        public string no_kwitansi;
        public string no_registrasi;
        public string tanggal_pembayaran;
        public string nama_penanggung_jawab;
        public string nama_abu;
        public string uang_jaminan;
        public tandaTerimaPembayaranJaminanData(string noKwitansi, string noRegistrasi, string tanggalPembayaran, string namaPenanggungJawab, string namaAbu, string uangJaminan)
        {
            no_kwitansi = noKwitansi;
            no_registrasi = noRegistrasi;
            tanggal_pembayaran = tanggalPembayaran;
            nama_penanggung_jawab = namaPenanggungJawab;
            nama_abu = namaAbu;
            uang_jaminan = uangJaminan;
        }
    }
}
