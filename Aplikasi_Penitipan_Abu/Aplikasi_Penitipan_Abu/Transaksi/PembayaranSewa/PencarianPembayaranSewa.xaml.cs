using System;
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
namespace Aplikasi_Penitipan_Abu.Transaksi.PembayaranSewa
{
    /// <summary>
    /// Interaction logic for PencarianPembayaranSewa.xaml
    /// </summary>
    public partial class PencarianPembayaranSewa : Window
    {

        MySqlConnection conn;
        Registrasi registrasi;
        public int selectedId { get; set; }
        DataTable data;
        public PencarianPembayaranSewa()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            loadDataGrid();
            selectedId = -1;
        }

        public void loadDataGrid(string filter = "")
        {
            data = new DataTable();

            MySqlCommand cmd = new MySqlCommand("SELECT ps.id AS 'ID', da.nama_abu AS 'Nama Abu', da.nama_alternatif_abu AS 'Nama Alternatif Abu', k.no_kotak AS 'No Kotak', ps.tanggal_awal AS 'Tanggal Sewa Awal', ps.tanggal_akhir AS 'Tanggal Sewa Akhir' FROM pembayaran_sewa ps LEFT JOIN penitipan p ON ps.id_penitipan = p.id LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN kotak k ON k.id = ps.id_kotak where (da.nama_abu like ?a or da.nama_alternatif_abu like ?a or k.no_kotak like ?a) and ps.status = 0", conn);
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
            loadDataGrid(FilterRegistrasi.Text);
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
