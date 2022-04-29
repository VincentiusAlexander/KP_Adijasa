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


namespace Aplikasi_Penitipan_Abu.Transaksi
{
    /// <summary>
    /// Interaction logic for PencarianRegistrasi_pengambilan_abu.xaml
    /// </summary>
    public partial class PencarianRegistrasi_pengambilan_abu : Window
    {
        public int selectedId { get; set; }

        MySqlConnection conn;
        DataTable data;
        ArrayList listRegistrasi = new ArrayList();
        public PencarianRegistrasi_pengambilan_abu()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            selectedId = -1;
            loadDataGrid();
        }

        public void loadDataGrid(string filter = "")
        {
            data = new DataTable();

            MySqlCommand cmd = new MySqlCommand("SELECT p.id as 'ID', k.no_kotak AS 'No Kotak', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', da.alamat_abu AS 'Alamat Abu', da.jenis_kelamin AS 'Jenis Kelamin',  DATE(da.tanggal_lahir) AS 'Tanggal Lahir',  DATE(da.tanggal_wafat) AS 'Tanggal Wafat', DATE(da.tanggal_kremasi) AS 'Tanggal Kremasi', da.keterangan AS 'Keterangan', pj1.nama AS 'Nama Penanggung Jawab 1', pj2.nama AS 'Nama Penanggung Jawab 2' FROM penitipan p LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN kotak k ON k.id = p.kotak_id LEFT JOIN penanggung_jawab pj1 ON p.penanggung_jawab_satu_id = pj1.id LEFT JOIN penanggung_jawab pj2 ON p.penanggung_jawab_dua_id = pj2.id WHERE (k.no_kotak LIKE ?a or da.nama_abu LIKE ?a or da.nama_alternatif_abu LIKE ?a or da.alamat_abu LIKE ?a or da.jenis_kelamin LIKE ?a or pj1.nama LIKE ?a or pj2.nama LIKE ?a) and p.id not in (select id_penitipan from pengambilan_abu)", conn);
            cmd.Parameters.AddWithValue("?a", "%" + filter + "%");
            conn.Close();
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(data);
            dtGrid.ItemsSource = data.DefaultView;
        }

        private void FilterRegistrasi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                loadDataGrid(FilterRegistrasi.Text);
            }
        }

        private void PilihRegistrasi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow dr = data.Rows[dtGrid.SelectedIndex];
                selectedId = Int32.Parse(dr["ID"].ToString());
                this.Close();
            }
            catch (Exception)
            {
                this.Close();
            }
        }
    }
}
