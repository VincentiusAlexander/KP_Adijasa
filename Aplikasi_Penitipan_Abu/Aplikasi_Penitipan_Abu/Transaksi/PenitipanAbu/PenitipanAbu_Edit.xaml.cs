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

namespace Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu
{
    /// <summary>
    /// Interaction logic for PenitipanAbu_Edit.xaml
    /// </summary>
    public partial class PenitipanAbu_Edit : Page
    {
        MySqlConnection conn;
        int id_penitipan_abu = -1;
        bool error;
        ArrayList listKotak = new ArrayList();
        int id_data_abu = -1;
        int id_kotak = -1;
        int id_penanggung_jawab_satu = -1;
        int id_penanggung_jawab_dua = -1;
        public PenitipanAbu_Edit()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        private void btn_cari_data_penitipan_abu_Click(object sender, RoutedEventArgs e)
        {
            PencarianRegistrasi pencarianRegistrasi = new PencarianRegistrasi();
            pencarianRegistrasi.ShowDialog();
            id_penitipan_abu = pencarianRegistrasi.selectedId;
            if (id_penitipan_abu == 0)
            {
                System.Windows.Forms.MessageBox.Show("Pencarian Gagal, ulang kembali pencarian", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            resetInput();
            tb_noreg_penitipan_abu.Text = id_penitipan_abu.ToString();
            MySqlCommand cmd = new MySqlCommand("select * from penitipan where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_penitipan_abu);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id_kotak = reader.GetInt32(4);
                id_data_abu = reader.GetInt32(5);
                id_penanggung_jawab_satu = reader.GetInt32(6);
                id_penanggung_jawab_dua = reader.GetInt32(7);
                tb_tgl_registrasi.Text = reader.GetDateTime(1).ToString("dd/MM/yyyy");
                dp_tanggal_titip.SelectedDate = reader.GetDateTime(2);
                dp_tanggal_ambil.SelectedDate = reader.GetDateTime(3);
            }
            reader.Close();
            conn.Close();
            int status = 0;
            cmd = new MySqlCommand("select id, kategori_id, no_kotak from kotak where status = ?status", conn);
            cmd.Parameters.AddWithValue("?status", status);
            conn.Close();
            conn.Open();
            reader = cmd.ExecuteReader();
            cb_kotak.DisplayMemberPath = "nama";
            cb_kotak.SelectedValuePath = "id";
            listKotak.Clear();
            int index = -1, ctr = 0;
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string nama = reader.GetValue(2).ToString();
                int kategori_id = (int)reader.GetValue(1);
                listKotak.Add(new Kotak(id, kategori_id, nama));
                if (id_kotak.Equals(reader.GetValue(0)))
                {
                    index = ctr;
                }
                ctr++;
            }
            reader.Close();
            conn.Close();
            if (listKotak.Count <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Kotak belum tersedia, buat kotak terlebih dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            cb_kotak.ItemsSource = listKotak;
            cb_kotak.SelectedIndex = index;
            cmd = new MySqlCommand("select * from data_abu where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_data_abu);
            conn.Close();
            conn.Open();
            cb_jk.Items.Clear();
            cb_jk.Items.Add("Laki-laki");
            cb_jk.Items.Add("Perempuan");
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tb_nama_abu.Text = reader.GetString(1);
                tb_nama_alternatif_abu.Text = reader.GetString(2);
                tb_alamat_abu.Text = reader.GetString(3);
                if (reader.GetString(4) == "Laki-laki")
                {
                    cb_jk.SelectedIndex = 0;
                }
                else
                {
                    cb_jk.SelectedIndex = 1;
                }
                dp_tanggal_lahir.SelectedDate = reader.GetDateTime(5);
                dp_tanggal_wafat.SelectedDate = reader.GetDateTime(6);
                dp_tanggal_kremasi.SelectedDate = reader.GetDateTime(7);
                tb_keterangan.Text = reader.GetString(8);
            }
            reader.Close();
            conn.Close();
            if (id_penanggung_jawab_satu != -1)
            {
                cmd = new MySqlCommand("select * from penanggung_jawab where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", id_penanggung_jawab_satu);
                conn.Close();
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tb_nama_p1.Text = reader.GetString(1);
                    tb_alamat_p1.Text = reader.GetString(2);
                    tb_nomor_p1.Text = reader.GetString(3);
                    tb_relasi_p1.Text = reader.GetString(4);
                }
                reader.Close();
                conn.Close();
            }
            if (id_penanggung_jawab_dua != -1)
            {
                cmd = new MySqlCommand("select * from penanggung_jawab where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", id_penanggung_jawab_dua);
                conn.Close();
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tb_nama_p2.Text = reader.GetString(1);
                    tb_alamat_p2.Text = reader.GetString(2);
                    tb_nomor_p2.Text = reader.GetString(3);
                    tb_relasi_p2.Text = reader.GetString(4);
                }
                reader.Close();
                conn.Close();
            }
        }

        private void cb_kotak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listKotak.Count <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Kotak belum tersedia, buat kotak terlebih dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            if (id_penitipan_abu != 0)
            {
                Kotak temp = (Kotak)listKotak[cb_kotak.SelectedIndex];
                int id_kategori = temp.kategori_id;
                MySqlCommand cmd = new MySqlCommand("select harga from kategori where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", id_kategori);
                conn.Close();
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tb_harga_kotak.Text = reader.GetInt32(0).ToString();
                }
                reader.Close();
                conn.Close();
            }
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            if (id_penitipan_abu == -1 || id_penitipan_abu == 0)
            {
                System.Windows.Forms.MessageBox.Show("Pilih dulu data yang ingin diubah", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            error = false;
            cekInput();
            if (error)
            {
                MessageBox.Show("Data tidak lengkap!");
                return;
            }
            Kotak temp = (Kotak)listKotak[cb_kotak.SelectedIndex];
            int kotak_id = temp.id;
            MySqlCommand cmd = new MySqlCommand("select * from kotak where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", kotak_id);
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
            cmd = new MySqlCommand("update penitipan set tanggal_titip = ?tanggal_titip, tanggal_ambil = ?tanggal_ambil, kotak_id = ?kotak_id where id = ?id", conn);
            cmd.Parameters.AddWithValue("?tanggal_titip", dp_tanggal_titip.SelectedDate.Value);
            cmd.Parameters.AddWithValue("?tanggal_ambil", dp_tanggal_ambil.SelectedDate.Value);
            cmd.Parameters.AddWithValue("?kotak_id", kotak_id);
            cmd.Parameters.AddWithValue("?id", id_penitipan_abu);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            cmd = new MySqlCommand("update data_abu set nama_abu = ?nama_abu, nama_alternatif_abu = ?nama_alternatif_abu, alamat_abu = ?alamat_abu, jenis_kelamin = ?jenis_kelamin, tanggal_lahir = ?tanggal_lahir, tanggal_wafat = ?tanggal_wafat, tanggal_kremasi = ?tanggal_kremasi, keterangan = ?keterangan where id = ?id", conn);
            cmd.Parameters.AddWithValue("?nama_abu", tb_nama_abu.Text);
            cmd.Parameters.AddWithValue("?nama_alternatif_abu", tb_nama_alternatif_abu.Text);
            cmd.Parameters.AddWithValue("?alamat_abu", tb_alamat_abu.Text);
            cmd.Parameters.AddWithValue("?jenis_kelamin", cb_jk.Text);
            cmd.Parameters.AddWithValue("?tanggal_lahir", dp_tanggal_lahir.SelectedDate.Value);
            cmd.Parameters.AddWithValue("?tanggal_wafat", dp_tanggal_wafat.SelectedDate.Value);
            cmd.Parameters.AddWithValue("?tanggal_kremasi", dp_tanggal_kremasi.SelectedDate.Value);
            cmd.Parameters.AddWithValue("?keterangan", tb_keterangan.Text);
            cmd.Parameters.AddWithValue("?id", id_penitipan_abu);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            if (id_penanggung_jawab_satu != -1)
            {
                cmd = new MySqlCommand("update penanggung_jawab set nama = ?nama, alamat = ?alamat, nomor_telp = ?nomor_telp, relasi = ?relasi where id = ?id", conn);
                cmd.Parameters.AddWithValue("?nama", tb_nama_p1.Text);
                cmd.Parameters.AddWithValue("?alamat", tb_alamat_p1.Text);
                cmd.Parameters.AddWithValue("?nomor_telp", tb_nomor_p1.Text);
                cmd.Parameters.AddWithValue("?relasi", tb_relasi_p1.Text);
                cmd.Parameters.AddWithValue("?id", id_penanggung_jawab_satu);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            if (id_penanggung_jawab_dua != -1)
            {
                cmd = new MySqlCommand("update penanggung_jawab set nama = ?nama, alamat = ?alamat, nomor_telp = ?nomor_telp, relasi = ?relasi where id = ?id", conn);
                cmd.Parameters.AddWithValue("?nama", tb_nama_p2.Text);
                cmd.Parameters.AddWithValue("?alamat", tb_alamat_p2.Text);
                cmd.Parameters.AddWithValue("?nomor_telp", tb_nomor_p2.Text);
                cmd.Parameters.AddWithValue("?relasi", tb_relasi_p2.Text);
                cmd.Parameters.AddWithValue("?id", id_penanggung_jawab_dua);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            System.Windows.Forms.MessageBox.Show("Pengubahan penitipan abu Berhasil !", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }

        private void cekInput()
        {
            String tanggalAwal = dp_tanggal_titip.SelectedDate.ToString();
            String tanggalAkhir = dp_tanggal_ambil.SelectedDate.ToString();
            String nama = tb_nama_p1.Text.ToString();
            String alamat = tb_alamat_p1.Text.ToString();
            String notelp = tb_nomor_p1.Text.ToString();
            String relasi = tb_relasi_p1.Text.ToString();
            String namaAbu = tb_nama_abu.Text.ToString();
            String alamatAbu = tb_alamat_abu.Text.ToString();
            String tglLahirAbu = dp_tanggal_lahir.SelectedDate.ToString();
            String tglWafatAbu = dp_tanggal_wafat.SelectedDate.ToString();
            String tglKremasiAbu = dp_tanggal_kremasi.SelectedDate.ToString();
            if (tanggalAkhir == "" || tanggalAwal == "" || nama == "" || alamat == "" || notelp == "" || relasi == "" ||
                namaAbu == "" || alamatAbu == "" || tglLahirAbu == "" || tglWafatAbu == "" || tglKremasiAbu == "") error = true;
        }

        private String textChanged(String text)
        {
            String temp = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsNumber(text[i]))
                {
                    temp += text[i];
                }
            }
            return temp;
        }

        private void tb_nomor_p1_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = tb_nomor_p1.Text;
            String temp = textChanged(text);
            tb_nomor_p1.Text = temp;
            tb_nomor_p1.SelectionStart = temp.Length;
        }

        private void tb_nomor_p2_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = tb_nomor_p2.Text;
            String temp = textChanged(text);
            tb_nomor_p2.Text = temp;
            tb_nomor_p2.SelectionStart = temp.Length;
        }

        private void btn_cetak_Click(object sender, RoutedEventArgs e)
        {
            if (id_penitipan_abu == -1)
            {
                System.Windows.Forms.MessageBox.Show("Pilih dulu data yang ingin dicetak", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            String nama = tb_nama_p1.Text.ToString();
            String alamat = tb_alamat_p1.Text.ToString();
            String notelp = tb_nomor_p1.Text.ToString();
            Kotak temp = (Kotak)listKotak[cb_kotak.SelectedIndex];
            String kotak = temp.nama;
            String tanggal_registrasi = tb_tgl_registrasi.Text;
            String namaAbu = tb_nama_abu.Text.ToString();
            String jk = cb_jk.SelectedItem.ToString();
            String tanggal_meninggal = dp_tanggal_wafat.SelectedDate.Value.ToString("dd/M/yyyy");
            String tanggal_kremasi = dp_tanggal_kremasi.SelectedDate.Value.ToString("dd/M/yyyy");
            TandaTerimaPenitipanAbuFix tandaTerimaPenitipanAbuFix = new TandaTerimaPenitipanAbuFix(nama, alamat, notelp, kotak, tanggal_registrasi, namaAbu, jk, tanggal_meninggal, tanggal_kremasi);
            tandaTerimaPenitipanAbuFix.Show();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (id_penitipan_abu.Equals(0) || id_penitipan_abu.Equals(-1))
            {
                System.Windows.Forms.MessageBox.Show("Pilih dulu data yang ingin dihapus", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            MySqlCommand cmd = new MySqlCommand("update penitipan set status = 1 where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", id_penitipan_abu);
            conn.Close();
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Penghapusan penitipan abu Berhasil !", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            resetInput();
        }
        private void resetInput()
        {
            tb_noreg_penitipan_abu.Text = "-";
            tb_tgl_registrasi.Text = "-";
            dp_tanggal_titip.SelectedDate = null;
            dp_tanggal_ambil.SelectedDate = null;
            dp_tanggal_lahir.SelectedDate = null;
            dp_tanggal_wafat.SelectedDate = null;
            dp_tanggal_kremasi.SelectedDate = null;
            cb_kotak.SelectedIndex = 0;
            tb_harga_kotak.Text = "-";
            tb_nama_abu.Text = "";
            tb_nama_alternatif_abu.Text = "";
            tb_alamat_abu.Text = "";
            cb_jk.SelectedIndex = 0;
            tb_keterangan.Text = "";
            tb_nama_p1.Text = "";
            tb_alamat_p1.Text = "";
            tb_nomor_p1.Text = "";
            tb_relasi_p1.Text = "";
            tb_nama_p2.Text = "";
            tb_alamat_p2.Text = "";
            tb_nomor_p2.Text = "";
            tb_relasi_p2.Text = "";
        }

        private void dp_tanggal_ambil_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tanggal_titip.SelectedDate != null && dp_tanggal_ambil.SelectedDate != null)
            {
                if (dp_tanggal_ambil.SelectedDate < dp_tanggal_titip.SelectedDate)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Ambil kurang dari Tanggal Simpan!");
                    dp_tanggal_ambil.SelectedDate = dp_tanggal_titip.SelectedDate;
                    return;
                }
            }
        }

        private void dp_tanggal_lahir_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tanggal_lahir.SelectedDate != null)
            {
                if (dp_tanggal_lahir.SelectedDate > DateTime.Now)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Lahir melebihi Tanggal Hari Ini!");
                    dp_tanggal_lahir.SelectedDate = DateTime.Now;
                    return;
                }
            }
        }

        private void dp_tanggal_wafat_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tanggal_wafat.SelectedDate != null && dp_tanggal_lahir.SelectedDate != null)
            {
                if (dp_tanggal_wafat.SelectedDate > DateTime.Now || dp_tanggal_wafat.SelectedDate < dp_tanggal_lahir.SelectedDate)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Wafat melebihi Tanggal Hari Ini!");
                    dp_tanggal_wafat.SelectedDate = dp_tanggal_lahir.SelectedDate;
                    return;
                }
            }
        }

        private void dp_tanggal_kremasi_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tanggal_wafat.SelectedDate != null && dp_tanggal_kremasi.SelectedDate != null)
            {
                if (dp_tanggal_wafat.SelectedDate > dp_tanggal_kremasi.SelectedDate)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Kremasi kurang dari Tanggal Wafat!");
                    dp_tanggal_kremasi.SelectedDate = dp_tanggal_wafat.SelectedDate;
                    return;
                }
            }
        }
    }
}
