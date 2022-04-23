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

namespace Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu
{
    /// <summary>
    /// Interaction logic for PenitipanAbu_Add.xaml
    /// </summary>
    public partial class PenitipanAbu_Add : Page
    {
        MySqlConnection conn;
        public PenitipanAbu_Add()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            loadData();
        }

        private void btn_cetak_form_penitipan_abu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu.PenitipanAbu_Form());
        }

        private void loadData()
        {
            tb_tglreg_penitipan_abu.Text = DateTime.Now.ToString("dd/MM/yyyy");
            cb_jk_abu_penitipan_abu.Items.Add("Laki-laki");
            cb_jk_abu_penitipan_abu.Items.Add("Perempuan");
            cb_jk_abu_penitipan_abu.SelectedIndex = 0;
        }

        private void inputDataAbu()
        {
            String namaAbu = tb_nama_abu_penitipan_abu.Text.ToString();
            String namaAlternatifAbu = tb_nama_alternatif_abu_penitipan_abu.Text.ToString();
            String alamatAbu = tb_alamat_abu_penitipan_abu.Text.ToString();
            String jkAbu = cb_jk_abu_penitipan_abu.Text.ToString();
            DateTime tglLahirAbu = dp_tgl_lahir_abu_penitipan_abu.SelectedDate.Value;
            DateTime tglWafatAbu = dp_tgl_wafat_abu_penitipan_abu.SelectedDate.Value;
            DateTime tglKremasiAbu = dp_tgl_kremasi_abu_penitipan_abu.SelectedDate.Value;
            String keteranganAbu = tb_keterangan_abu_penitipan_abu.Text.ToString();
            MySqlCommand cmd = new MySqlCommand("insert into data_abu (nama_abu, nama_alternatif_abu, alamat_abu, jenis_kelamin, tanggal_lahir, tanggal_wafat, tanggal_kremasi, keterangan) values (?namaAbu, ?namaAlternatifAbu, ?alamatAbu, ?jkAbu, ?tglLahirAbu, ?tglWafatAbu, ?tglKremasiAbu, ?keteranganAbu)", conn);
            cmd.Parameters.AddWithValue("?namaAbu", namaAbu);
            cmd.Parameters.AddWithValue("?namaAlternatifAbu", namaAlternatifAbu);
            cmd.Parameters.AddWithValue("?alamatAbu", alamatAbu);
            cmd.Parameters.AddWithValue("?jkAbu", jkAbu);
            cmd.Parameters.AddWithValue("?tglLahirAbu", tglLahirAbu);
            cmd.Parameters.AddWithValue("?tglWafatAbu", tglWafatAbu);
            cmd.Parameters.AddWithValue("?tglKremasiAbu", tglKremasiAbu);
            cmd.Parameters.AddWithValue("?keteranganAbu", keteranganAbu);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            //MessageBox.Show("berhasil");
        }

        private void btn_cetak_tanda_terima_abu_Click(object sender, RoutedEventArgs e)
        {
            inputDataAbu();
            inputDataPenanggungJawab();
            this.NavigationService.Navigate(new Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu.TandaTerimaPenitipanAbu());
        }

        private void inputDataPenanggungJawab()
        {
            String nama = tb_nama_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String alamat = tb_alamat_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String notelp = tb_notelp_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String relasi = tb_relasi_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            MySqlCommand cmd = new MySqlCommand("insert into penanggung_jawab (nama, alamat, nomor_telp, relasi) values (?nama, ?alamat, ?notelp, ?relasi)", conn);
            cmd.Parameters.AddWithValue("?nama", nama);
            cmd.Parameters.AddWithValue("?alamat", alamat);
            cmd.Parameters.AddWithValue("?notelp", notelp);
            cmd.Parameters.AddWithValue("?relasi", relasi);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            nama = tb_nama_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            alamat = tb_alamat_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            notelp = tb_notelp_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            relasi = tb_relasi_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            cmd = new MySqlCommand("insert into penanggung_jawab (nama, alamat, nomor_telp, relasi) values (?nama, ?alamat, ?notelp, ?relasi)", conn);
            cmd.Parameters.AddWithValue("?nama", nama);
            cmd.Parameters.AddWithValue("?alamat", alamat);
            cmd.Parameters.AddWithValue("?notelp", notelp);
            cmd.Parameters.AddWithValue("?relasi", relasi);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            //MessageBox.Show("berhasil");
        }
    }
}
