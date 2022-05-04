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
    /// Interaction logic for PembayaranJaminan_Edit.xaml
    /// </summary>
    public partial class PembayaranJaminan_Edit : Page
    {
        MySqlConnection conn;
        int selectedId = -1;
        public PembayaranJaminan_Edit()
        {
            InitializeComponent();
            populateComboBox();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }
        private void populateComboBox()
        {
            cb_status.Items.Insert(0, "Belum Terbayar");
            cb_status.Items.Insert(1, "Sudah Terbayar");
            cb_status.Items.Insert(2, "Dikembalikan Pada Pemilik");
            cb_status.SelectedIndex = 0;
        }
        private void btn_cari_jaminan_Click(object sender, RoutedEventArgs e)
        {
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
            int status = -1;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_penitipan = reader.GetInt32(1);
                total_jaminan = reader.GetInt32(2);
                if (reader.GetInt32(4) == 1)
                {
                    status = 2;
                }else if (reader.GetInt32(3) == 0)
                {
                    status = 0;
                }else if (reader.GetInt32(3) == 1)
                {
                    status = 1;
                }
            }
            reader.Close();
            uang_jaminan_txt.Text = "Rp." + total_jaminan.ToString();
            no_kwitansi_jaminan_txt.Text = selectedId.ToString();
            no_registrasi_txt.Text = id_penitipan.ToString();
            cb_status.SelectedIndex = status;

            cmd.Parameters.Clear();
            cmd.CommandText = "select pj.nama from penitipan p left join penanggung_jawab pj on p.penanggung_jawab_satu_id = pj.id where p.id = ?id";
            cmd.Parameters.AddWithValue("?id", id_penitipan);
            reader = cmd.ExecuteReader();
            string nama = "";
            while (reader.Read())
            {
                nama = reader.GetString(0);
            }
            nama_penanggung_jawab_txt.Text = nama;
            conn.Close();
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            if (check_box_centang_bila_data_benar.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show("Centang Dahulu checkbox yang tersedia", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("update jaminan set status = ?status, dikembalikan = ?dikembalikan where id = ?id", conn);
            int status = -1;
            int dikembalikan = -1;
            if (cb_status.SelectedIndex == 0)
            {
                status = 0;
                dikembalikan = 0;
            }
            else if (cb_status.SelectedIndex == 1)
            {
                status = 1;
                dikembalikan = 0;
            }
            else if (cb_status.SelectedIndex == 2)
            {
                status = 1;
                dikembalikan = 1;
            }
            cmd.Parameters.AddWithValue("?status", status);
            cmd.Parameters.AddWithValue("?dikembalikan", dikembalikan);
            cmd.Parameters.AddWithValue("?id", selectedId);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Berhasil Edit", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
