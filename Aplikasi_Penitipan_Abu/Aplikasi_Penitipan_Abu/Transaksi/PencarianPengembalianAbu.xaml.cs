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
    /// Interaction logic for PencarianPengembalianAbu.xaml
    /// </summary>
    public partial class PencarianPengembalianAbu : Window
    {
        public int selectedId { get; set; }

        MySqlConnection conn;
        DataTable data;
        public PencarianPengembalianAbu()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            loadDataGrid();
        }
        public void loadDataGrid(string filter = "")
        {
            data = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SELECT pa.id AS 'ID', da.nama_abu AS 'Nama Abu', pj.nama AS 'Nama Penanggung Jawab', case when pa.status = 0 then 'Aktif' when pa.status = 1 then 'Terhapus' end as 'Status' FROM pengambilan_abu pa LEFT JOIN penitipan p ON pa.id_penitipan = p.id LEFT JOIN data_abu da ON da.id = p.data_abu_id LEFT JOIN penanggung_jawab pj ON p.penanggung_jawab_satu_id = pj.id where pa.id = ?a or da.nama_abu like ?a or pj.nama like ?a", conn);
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
