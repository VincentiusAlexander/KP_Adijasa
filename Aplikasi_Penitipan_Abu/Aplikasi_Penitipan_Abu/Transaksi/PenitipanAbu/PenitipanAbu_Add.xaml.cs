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
    /// Interaction logic for PenitipanAbu_Add.xaml
    /// </summary>
    public partial class PenitipanAbu_Add : Page
    {
        MySqlConnection conn;
        ArrayList listKotak = new ArrayList();
        Boolean error, save;
        int ctr;

        public PenitipanAbu_Add()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            loadData();
        }

        private void btn_cetak_form_penitipan_abu_Click(object sender, RoutedEventArgs e)
        {
            PenitipanAbu_FormFix penitipanAbu_FormFix = new PenitipanAbu_FormFix();
            penitipanAbu_FormFix.Show();
        }

        private void loadData()
        {
            tb_tglreg_penitipan_abu.Text = DateTime.Now.ToString("dd/MM/yyyy");
            cb_jk_abu_penitipan_abu.Items.Add("Laki-laki");
            cb_jk_abu_penitipan_abu.Items.Add("Perempuan");
            cb_jk_abu_penitipan_abu.SelectedIndex = 0;
            initRegistrasi();
            error = false;
            save = false;
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
        }

        private void btn_cetak_tanda_terima_abu_Click(object sender, RoutedEventArgs e)
        {
            if (!save)
            {
                System.Windows.Forms.MessageBox.Show("Belum melakukan penyimpanan data", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                String nama = tb_nama_penanggung_jawab_satu_penitipan_abu.Text.ToString();
                String alamat = tb_alamat_penanggung_jawab_satu_penitipan_abu.Text.ToString();
                String notelp = tb_notelp_penanggung_jawab_satu_penitipan_abu.Text.ToString();
                Kotak temp = (Kotak)listKotak[cb_no_kotak_penitipan_abu.SelectedIndex];
                String kotak = temp.nama;
                String tanggal_registrasi = DateTime.Now.ToString("dd/MM/yyyy");
                String namaAbu = tb_nama_abu_penitipan_abu.Text.ToString();
                String jk = cb_jk_abu_penitipan_abu.SelectedItem.ToString();
                String tanggal_meninggal = dp_tgl_wafat_abu_penitipan_abu.SelectedDate.Value.ToString("dd/MM/yyyy");
                String tanggal_kremasi = dp_tgl_kremasi_abu_penitipan_abu.SelectedDate.Value.ToString("dd/MM/yyyy");
                TandaTerimaPenitipanAbuFix tandaTerimaPenitipanAbuFix = new TandaTerimaPenitipanAbuFix(nama, alamat, notelp, kotak, tanggal_registrasi, namaAbu, jk, tanggal_meninggal, tanggal_kremasi);
                tandaTerimaPenitipanAbuFix.Show(); 
            }
        }

        private void initRegistrasi(int status = 0)
        {
            MySqlCommand cmd = new MySqlCommand("select max(id)+1 from penitipan", conn);
            conn.Close();
            conn.Open();
            tb_noreg_penitipan_abu.Text = Int32.Parse(cmd.ExecuteScalar().ToString()).ToString();
            cmd = new MySqlCommand("select id, kategori_id, no_kotak from kotak where status = ?status", conn);
            cmd.Parameters.AddWithValue("?status", status);
            conn.Close();
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            cb_no_kotak_penitipan_abu.DisplayMemberPath = "nama";
            cb_no_kotak_penitipan_abu.SelectedValuePath = "id";
            listKotak.Clear();
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string nama = reader.GetValue(2).ToString();
                int kategori_id = (int)reader.GetValue(1);
                listKotak.Add(new Kotak(id, kategori_id, nama));
            }
            reader.Close();
            conn.Close();
            if (listKotak.Count <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Kotak belum tersedia, buat kotak terlebih dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
            cb_no_kotak_penitipan_abu.ItemsSource = listKotak;
            cb_no_kotak_penitipan_abu.SelectedIndex = 0;
            Kotak temp = (Kotak)listKotak[cb_no_kotak_penitipan_abu.SelectedIndex];
            int id_kategori = temp.kategori_id;
            cmd = new MySqlCommand("select harga from kategori where id = ?id", conn);
            conn.Close();
            conn.Open();
            cmd.Parameters.AddWithValue("?id", id_kategori);
            tb_harga_kotak_penitipan_abu.Text = ((int)cmd.ExecuteScalar()).ToString();
            conn.Close();
        }

        private void inputDataPenanggungJawab()
        {
            MySqlCommand cmd;
            String nama = tb_nama_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String alamat = tb_alamat_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String notelp = tb_notelp_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String relasi = tb_relasi_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            if (nama != "" || alamat != "" || notelp != "" || relasi != "")
            {
                cmd = new MySqlCommand("insert into penanggung_jawab (nama, alamat, nomor_telp, relasi) values (?nama, ?alamat, ?notelp, ?relasi)", conn);
                cmd.Parameters.AddWithValue("?nama", nama);
                cmd.Parameters.AddWithValue("?alamat", alamat);
                cmd.Parameters.AddWithValue("?notelp", notelp);
                cmd.Parameters.AddWithValue("?relasi", relasi);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                ctr++;
            }
            nama = tb_nama_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            alamat = tb_alamat_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            notelp = tb_notelp_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            relasi = tb_relasi_penanggung_jawab_dua_penitipan_abu.Text.ToString();
            if (nama != "" || alamat != "" || notelp != "" || relasi != "")
            {
                cmd = new MySqlCommand("insert into penanggung_jawab (nama, alamat, nomor_telp, relasi) values (?nama, ?alamat, ?notelp, ?relasi)", conn);
                cmd.Parameters.AddWithValue("?nama", nama);
                cmd.Parameters.AddWithValue("?alamat", alamat);
                cmd.Parameters.AddWithValue("?notelp", notelp);
                cmd.Parameters.AddWithValue("?relasi", relasi);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                ctr++;
            }
        }

        private void cb_no_kotak_penitipan_abu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Kotak temp = (Kotak)listKotak[cb_no_kotak_penitipan_abu.SelectedIndex];
            int id_kategori = temp.kategori_id;
            MySqlCommand cmd = new MySqlCommand("select harga from kategori where id = ?id", conn);
            conn.Close();
            conn.Open();
            cmd.Parameters.AddWithValue("?id", id_kategori);
            tb_harga_kotak_penitipan_abu.Text = ((int)cmd.ExecuteScalar()).ToString();
            conn.Close();
        }
        private void cekInput()
        {
            String tanggalAwal = dp_tanggal_simpan_penitipan_abu.SelectedDate.ToString();
            String tanggalAkhir = dp_tanggal_ambil_penitipan_abu.SelectedDate.ToString();
            String nama = tb_nama_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String alamat = tb_alamat_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String notelp = tb_notelp_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String relasi = tb_relasi_penanggung_jawab_satu_penitipan_abu.Text.ToString();
            String namaAbu = tb_nama_abu_penitipan_abu.Text.ToString();
            String alamatAbu = tb_alamat_abu_penitipan_abu.Text.ToString();
            String tglLahirAbu = dp_tgl_lahir_abu_penitipan_abu.SelectedDate.ToString();
            String tglWafatAbu = dp_tgl_wafat_abu_penitipan_abu.SelectedDate.ToString();
            String tglKremasiAbu = dp_tgl_kremasi_abu_penitipan_abu.SelectedDate.ToString();
            if (tanggalAkhir == "" || tanggalAwal == "" || nama == "" || alamat == "" || notelp == "" || relasi == "" ||
                namaAbu == "" || alamatAbu == "" || tglLahirAbu == "" || tglWafatAbu == "" || tglKremasiAbu == "") error = true;
        }

        private void tb_notelp_penanggung_jawab_satu_penitipan_abu_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = tb_notelp_penanggung_jawab_satu_penitipan_abu.Text;
            String temp = textChanged(text);
            tb_notelp_penanggung_jawab_satu_penitipan_abu.Text = temp;
            tb_notelp_penanggung_jawab_satu_penitipan_abu.SelectionStart = temp.Length;
        }

        private void tb_notelp_penanggung_jawab_dua_penitipan_abu_TextChanged(object sender, TextChangedEventArgs e)
        {
            String text = tb_notelp_penanggung_jawab_dua_penitipan_abu.Text;
            String temp = textChanged(text);
            tb_notelp_penanggung_jawab_dua_penitipan_abu.Text = temp;
            tb_notelp_penanggung_jawab_dua_penitipan_abu.SelectionStart = temp.Length;
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

        private void dp_tanggal_ambil_penitipan_abu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tanggal_simpan_penitipan_abu.SelectedDate != null && dp_tanggal_ambil_penitipan_abu.SelectedDate != null)
            {
                if (dp_tanggal_ambil_penitipan_abu.SelectedDate < dp_tanggal_simpan_penitipan_abu.SelectedDate)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Ambil kurang dari Tanggal Simpan!");
                    dp_tanggal_ambil_penitipan_abu.SelectedDate = dp_tanggal_simpan_penitipan_abu.SelectedDate;
                    return;
                }
            }
        }

        private void dp_tgl_lahir_abu_penitipan_abu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tgl_lahir_abu_penitipan_abu.SelectedDate != null)
            {
                if (dp_tgl_lahir_abu_penitipan_abu.SelectedDate > DateTime.Now)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Lahir melebihi Tanggal Hari Ini!");
                    dp_tgl_lahir_abu_penitipan_abu.SelectedDate = DateTime.Now;
                    return;
                }
            }
        }

        private void dp_tgl_wafat_abu_penitipan_abu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tgl_wafat_abu_penitipan_abu.SelectedDate != null && dp_tgl_lahir_abu_penitipan_abu.SelectedDate != null)
            {
                if (dp_tgl_wafat_abu_penitipan_abu.SelectedDate > DateTime.Now || dp_tgl_wafat_abu_penitipan_abu.SelectedDate < dp_tgl_lahir_abu_penitipan_abu.SelectedDate)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Wafat melebihi Tanggal Hari Ini!");
                    dp_tgl_wafat_abu_penitipan_abu.SelectedDate = dp_tgl_lahir_abu_penitipan_abu.SelectedDate;
                    return;
                }
            }
        }

        private void dp_tgl_kremasi_abu_penitipan_abu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tgl_wafat_abu_penitipan_abu.SelectedDate != null && dp_tgl_kremasi_abu_penitipan_abu.SelectedDate != null)
            {
                if (dp_tgl_wafat_abu_penitipan_abu.SelectedDate > dp_tgl_kremasi_abu_penitipan_abu.SelectedDate)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Kremasi kurang dari Tanggal Wafat!");
                    dp_tgl_kremasi_abu_penitipan_abu.SelectedDate = dp_tgl_wafat_abu_penitipan_abu.SelectedDate;
                    return;
                }
            }
        }

        private void dp_tanggal_simpan_penitipan_abu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dp_tanggal_simpan_penitipan_abu.SelectedDate != null)
            {
                if (dp_tanggal_simpan_penitipan_abu.SelectedDate < DateTime.Today)
                {
                    System.Windows.Forms.MessageBox.Show("Tanggal Lahir melebihi Tanggal Hari Ini!");
                    dp_tanggal_simpan_penitipan_abu.SelectedDate = DateTime.Today;
                    return;
                }
            }
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            dp_tanggal_simpan_penitipan_abu.SelectedDate = null;
            dp_tanggal_ambil_penitipan_abu.SelectedDate = null;
            cb_no_kotak_penitipan_abu.SelectedIndex = 0;
            tb_nama_abu_penitipan_abu.Text = "";
            tb_nama_alternatif_abu_penitipan_abu.Text = "";
            tb_alamat_abu_penitipan_abu.Text = "";
            cb_jk_abu_penitipan_abu.SelectedIndex = 0;
            dp_tgl_lahir_abu_penitipan_abu.SelectedDate = null;
            dp_tgl_wafat_abu_penitipan_abu.SelectedDate = null;
            dp_tgl_kremasi_abu_penitipan_abu.SelectedDate = null;
            tb_keterangan_abu_penitipan_abu.Text = "";
            tb_alamat_penanggung_jawab_dua_penitipan_abu.Text = "";
            tb_alamat_penanggung_jawab_satu_penitipan_abu.Text = "";
            tb_notelp_penanggung_jawab_dua_penitipan_abu.Text = "";
            tb_notelp_penanggung_jawab_satu_penitipan_abu.Text = "";
            tb_nama_penanggung_jawab_dua_penitipan_abu.Text = "";
            tb_nama_penanggung_jawab_satu_penitipan_abu.Text = "";
            tb_relasi_penanggung_jawab_dua_penitipan_abu.Text = "";
            tb_relasi_penanggung_jawab_satu_penitipan_abu.Text = "";
            initRegistrasi();
        }

        private void btn_save_penitipan_abu_Click(object sender, RoutedEventArgs e)
        {
            error = false;
            ctr = 0;
            cekInput();
            if (error)
            {
                MessageBox.Show("Data tidak lengkap!");
                return;
            }
            if (!save)
            {
                DateTime tanggal_registrasi = DateTime.Now;
                DateTime tanggal_titip = dp_tanggal_simpan_penitipan_abu.SelectedDate.Value;
                DateTime tanggal_ambil = dp_tanggal_ambil_penitipan_abu.SelectedDate.Value;
                Kotak temp = (Kotak)listKotak[cb_no_kotak_penitipan_abu.SelectedIndex];
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
                inputDataAbu();
                inputDataPenanggungJawab();
                int data_abu_id = -1;
                int penanggung_jawab_satu_id = -1;
                int penanggung_jawab_dua_id = -1;
                cmd = new MySqlCommand("select id from data_abu order by id desc limit 1", conn);
                conn.Close();
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data_abu_id = (int)reader.GetValue(0);
                }
                reader.Close();
                conn.Close();
                if (ctr == 1)
                {
                    cmd = new MySqlCommand("select * from penanggung_jawab order by id desc limit 1", conn);
                    conn.Close();
                    conn.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        penanggung_jawab_satu_id = (int)reader.GetValue(0);
                    }
                    reader.Close();
                    conn.Close();
                }
                else
                {
                    int ctr2 = 0;
                    cmd = new MySqlCommand("select * from penanggung_jawab order by id desc limit 2", conn);
                    conn.Close();
                    conn.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (ctr2 > 0) penanggung_jawab_satu_id = (int)reader.GetValue(0);
                        else penanggung_jawab_dua_id = (int)reader.GetValue(0);
                        ctr2++;
                    }
                    reader.Close();
                    conn.Close();
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
                cmd = new MySqlCommand("select count(*)+1 from penitipan where kode_penitipan like ?tanggal", conn);
                cmd.Parameters.AddWithValue("?tanggal", "%" + tanggal + "%");
                conn.Close();
                conn.Open();
                int jumlah = Int32.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
                String kode = "MSK-" + tanggal + "-" + jumlah.ToString().PadLeft(5, '0');
                cmd = new MySqlCommand("insert into penitipan (tanggal_registrasi, tanggal_titip, tanggal_ambil, kotak_id, data_abu_id, penanggung_jawab_satu_id, penanggung_jawab_dua_id, status, kode_penitipan) values(?tanggal_registrasi, ?tanggal_titip, ?tanggal_ambil, ?kotak_id, ?data_abu_id, ?penanggung_jawab_satu_id, ?penanggung_jawab_dua_id, 0, ?kode_penitipan)", conn);
                cmd.Parameters.AddWithValue("?tanggal_registrasi", tanggal_registrasi);
                cmd.Parameters.AddWithValue("?tanggal_titip", tanggal_titip);
                cmd.Parameters.AddWithValue("?tanggal_ambil", tanggal_ambil);
                cmd.Parameters.AddWithValue("?kotak_id", kotak_id);
                cmd.Parameters.AddWithValue("?data_abu_id", data_abu_id);
                cmd.Parameters.AddWithValue("?penanggung_jawab_satu_id", penanggung_jawab_satu_id);
                cmd.Parameters.AddWithValue("?penanggung_jawab_dua_id", penanggung_jawab_dua_id);
                cmd.Parameters.AddWithValue("?kode_penitipan", kode);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                cmd = new MySqlCommand("update kotak set booking = 1 where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", kotak_id);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                System.Windows.Forms.MessageBox.Show("Save Penitipan Abu Berhasil !", "Success", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                save = true;
            }
        }
    }
    public class Kotak
    {
        public int id { get; set; }
        public string nama { get; set; }
        public int kategori_id { get; set; }

        public Kotak(int id, int kategori_id, string nama)
        {
            this.id = id;
            this.kategori_id = kategori_id;
            this.nama = nama;
        }
    }
}
