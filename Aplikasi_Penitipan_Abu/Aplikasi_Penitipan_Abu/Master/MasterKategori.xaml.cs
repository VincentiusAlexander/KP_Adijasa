using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
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
    /// Interaction logic for MasterKategori.xaml
    /// </summary>
    public partial class MasterKategori : Page
    {
        MySqlDataAdapter da;
        MySqlConnection conn;
        DataTable dt;
        int idEdit;
        public MasterKategori()
        {
            InitializeComponent();

            //loading connection for later uses
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");

            loadDataTable();
        }
        public void loadDataTable(int status = 0)
        {
            //load data in the data grid

            dt = new DataTable();
            MySqlCommand cmd = new MySqlCommand($"select id, nama, harga from kategori where status = ?status", conn);
            cmd.Parameters.AddWithValue("?status", status);
            conn.Close();
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            dtGrid.ItemsSource = dt.DefaultView;
            conn.Close();
        }



        private void dtGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //data grid handle double click button
            //get the selected index from data grid
            //get the id on the data grid to use in edit / delete purposes
            //show the data on the forms
            int selectedRow = dtGrid.SelectedIndex;
            if (selectedRow != -1)
            {
                DataRow dr = dt.Rows[selectedRow];
                NamaKategoriEdit.Text = dr[1].ToString();
                HargaKategoriEdit.Text = dr[2].ToString();
                idEdit = Int32.Parse(dr[0].ToString());
                txt_kategori_id.Text = idEdit.ToString();
            }
        }


        private void btnTambah_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nama = NamaKategoriAdd.Text;
                string harga = HargaKategoriAdd.Text;
                if (nama == "" || nama == null)
                {
                    System.Windows.Forms.MessageBox.Show("Isi Nama Kategori Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (harga == "" || harga == null)
                {
                    System.Windows.Forms.MessageBox.Show("Isi Harga Kategori Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    int test = Int32.Parse(harga);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Harga hanya boleh Number", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                MySqlCommand cmd = new MySqlCommand("insert into kategori (nama,harga) values (?nama,?harga)", conn);
                cmd.Parameters.AddWithValue("?nama", nama);
                cmd.Parameters.AddWithValue("?harga", harga);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                loadDataTable();
                System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Penambahan Kategori", "Sukses", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Gagal Memasukkan Data", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id;
                try
                {
                    id = Int32.Parse(txt_kategori_id.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Tekan Dua Kali Pada Salah Satu Item", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                MySqlCommand cmd = new MySqlCommand("update kategori set status = 1 where id = ?id", conn);
                cmd.Parameters.AddWithValue("?id", id);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                loadDataTable();
                System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Delete Kategori", "Sukses", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Gagal Melakukan Delete Data", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nama = NamaKategoriEdit.Text;
                string harga = HargaKategoriEdit.Text;
                int id;
                try
                {
                    id = Int32.Parse(txt_kategori_id.Text);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Tekan Dua Kali Pada Salah Satu Item", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (nama == "" || nama == null)
                {
                    System.Windows.Forms.MessageBox.Show("Isi Nama Kategori Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }
                if (harga == "" || harga == null)
                {
                    System.Windows.Forms.MessageBox.Show("Isi Harga Kategori Terlebih Dahulu", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    int test = Int32.Parse(harga);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Harga hanya boleh Number", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                MySqlCommand cmd = new MySqlCommand("update kategori set nama = ?nama, harga = ?harga where id = ?id", conn);
                cmd.Parameters.AddWithValue("?nama", nama);
                cmd.Parameters.AddWithValue("?harga", harga);
                cmd.Parameters.AddWithValue("?id", id);
                conn.Close();
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                loadDataTable();
                System.Windows.Forms.MessageBox.Show("Berhasil Melakukan Edit Kategori", "Sukses", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Gagal Melakukan Edit Data", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}
