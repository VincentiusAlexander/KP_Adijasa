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

namespace Aplikasi_Penitipan_Abu.Master
{
    /// <summary>
    /// Interaction logic for MasterKotak.xaml
    /// </summary>
    public partial class MasterKotak : Window
    {

        MySqlConnection conn;
        DataTable dt;
        ArrayList listKategori = new ArrayList();
        int idEdit;

        public MasterKotak()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            loadData();
        }

        public void loadData(int status = 0)
        {
            
            //populate the data grid
            dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand($"select kotak.id as 'ID Kotak', kotak.no_kotak as 'No Kotak', kategori.nama as 'Nama Kategori' from kotak left join kategori on kotak.kategori_id = kategori.id where kotak.status = ?status", conn);
            cmd.Parameters.AddWithValue("?status", status);
            conn.Close();
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            dtGrid.ItemsSource = dt.DefaultView;

            //populate the combobox
            cmd = new MySqlCommand("select id, nama, harga from kategori where status = ?status", conn);
            cmd.Parameters.AddWithValue("?status", status);

            MySqlDataReader reader = cmd.ExecuteReader();
            cb_kategori_add.DisplayMemberPath = "nama";
            cb_kategori_add.SelectedValuePath = "id";
            cb_kategori_edit.DisplayMemberPath = "nama";
            cb_kategori_edit.SelectedValuePath = "id";
            listKategori.Clear();
            while (reader.Read())
            {
                int id = (int)reader.GetValue(0);
                string nama = reader.GetValue(1).ToString();
                listKategori.Add(new Kategori(id, nama));
            }
            cb_kategori_add.ItemsSource = listKategori;
            cb_kategori_edit.ItemsSource = listKategori;
            if (cb_kategori_add.Items.Count > 0) cb_kategori_add.SelectedIndex = 0;
            if (cb_kategori_edit.Items.Count > 0) cb_kategori_edit.SelectedIndex = 0;


            conn.Close();
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string no_kotak = tb_no_kotak_add.Text;
                Kategori temp = (Kategori)listKategori[cb_kategori_add.SelectedIndex];
                int kategori_id = temp.id;
                if (no_kotak == "" || no_kotak == null)
                {
                    System.Windows.Forms.MessageBox.Show("Isi No Kotak Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (kategori_id == -1 )
                {
                    System.Windows.Forms.MessageBox.Show("Isi Kategori Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                MySqlCommand cmd = new MySqlCommand("insert into kotak (no_kotak,kategori_id) values (?no_kotak,?kategori_id)", conn);
                cmd.Parameters.AddWithValue("?no_kotak", no_kotak);
                cmd.Parameters.AddWithValue("?kategori_id", kategori_id);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                loadData();
                System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Penambahan Kategori", "Sukses", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Gagal Memasukkan Data", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void cb_kategori_add_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tampilkanHarga(cb_kategori_add);
        }

        private void tampilkanHarga(object sender)
        {
            ComboBox cb;
            Boolean isAdd = false;
            if (sender == cb_kategori_add)
            {
                cb = cb_kategori_add;
                isAdd = true;
            }
            else
            {
                cb = cb_kategori_edit;
                isAdd = false;
            }
            Kategori kotak = (Kategori) cb.SelectedItem;

            MySqlCommand cmd = new MySqlCommand("select harga from kategori where id = ?id", conn);
            cmd.Parameters.AddWithValue("?id", kotak.id);
            conn.Close();
            conn.Open();
        
            int harga = (int) cmd.ExecuteScalar();
            if (isAdd)
            {
                txt_harga_add.Text = harga.ToString();
            }
            else
            {
                txt_harga_edit.Text = harga.ToString();
            }
        }

        private void cb_kategori_edit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            tampilkanHarga(cb_kategori_edit);
        }

        private void dtGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedRow = dtGrid.SelectedIndex;
            if (selectedRow != -1)
            {
                DataRow dr = dt.Rows[selectedRow];
                no_kotak_edit.Text = dr[1].ToString();
                idEdit = Int32.Parse(dr[0].ToString());
                id_kotak_edit.Text = idEdit.ToString();
                MySqlCommand cmd = new MySqlCommand("select kategori_id from kotak where id = ?id", conn);
                conn.Close();
                conn.Open();

                cmd.Parameters.AddWithValue("?id", idEdit);

                int kategori_id = (int) cmd.ExecuteScalar();

                for (int i = 0; i < listKategori.Count; i++)
                {
                    Kategori element = (Kategori) listKategori[i];
                    if (kategori_id == element.id)
                    {
                        cb_kategori_edit.SelectedIndex = i;
                    }
                }
                conn.Close();

            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string no_kotak = no_kotak_edit.Text;
                Kategori temp = (Kategori)listKategori[cb_kategori_add.SelectedIndex];
                int kategori_id = temp.id;
                int id;
                try
                {
                    id = Int32.Parse(id_kotak_edit.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Tekan Dua Kali Pada Salah Satu Item", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (no_kotak == "" || no_kotak == null)
                {
                    System.Windows.Forms.MessageBox.Show("Isi No Kotak Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (kategori_id == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Isi Kategori Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                MySqlCommand cmd = new MySqlCommand("update kotak set status = 1 where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", id);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                loadData();
                System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Delete Kotak", "Sukses", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Gagal Delete Data", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string no_kotak = no_kotak_edit.Text;
                Kategori temp = (Kategori)listKategori[cb_kategori_add.SelectedIndex];
                int kategori_id = temp.id;
                int id;
                try
                {
                    id = Int32.Parse(id_kotak_edit.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Tekan Dua Kali Pada Salah Satu Item", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (no_kotak == "" || no_kotak == null)
                {
                    System.Windows.Forms.MessageBox.Show("Isi No Kotak Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (kategori_id == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Isi Kategori Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                MySqlCommand cmd = new MySqlCommand("update kotak set no_kotak = ?no_kotak, kategori_id = ?kategori_id where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", id);
                cmd.Parameters.AddWithValue("?no_kotak", no_kotak);
                cmd.Parameters.AddWithValue("?kategori_id", kategori_id);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                loadData();
                System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Edit Kotak", "Sukses", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Gagal Edit Data", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }

    public class Kategori
    {
        public int id { get; set; }
        public string nama { get; set; }

        public Kategori(int id, string nama)
        {
            this.id = id;
            this.nama = nama;
        }
    }
}
