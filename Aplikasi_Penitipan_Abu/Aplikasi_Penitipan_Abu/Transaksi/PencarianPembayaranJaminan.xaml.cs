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
    /// Interaction logic for PencarianPembayaranJaminan.xaml
    /// </summary>
    public partial class PencarianPembayaranJaminan : Window
    {
        public int selectedId { get; set; }

        MySqlConnection conn;
        DataTable data;
        public PencarianPembayaranJaminan()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            loadDataGrid();
        }

        public void loadDataGrid(string filter = "")
        {
            data = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SELECT j.id AS 'ID', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', j.total_jaminan AS 'Total Jaminan', ps.tanggal_awal AS 'Tanggal Awal Penyimpanan', ps.tanggal_akhir AS 'Tanggal Akhir Penyimpanan', case when j.status = 0 then 'Belum Terbayar' when j.status = 1 then 'Sudah Terbayar' end as 'Status' FROM jaminan j LEFT JOIN penitipan p ON p.id = j.id_penitipan LEFT JOIN pembayaran_sewa ps ON p.id = ps.id_penitipan LEFT JOIN data_abu da ON da.id = p.data_abu_id where j.id like ?a or da.nama_abu like ?a or da.nama_alternatif_abu like ?a or j.total_jaminan like ?a or ps.tanggal_awal like ?a or ps.tanggal_akhir like ?a", conn);
            conn.Open();
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
