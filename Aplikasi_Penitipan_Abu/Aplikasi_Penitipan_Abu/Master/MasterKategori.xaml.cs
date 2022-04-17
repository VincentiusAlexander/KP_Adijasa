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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Aplikasi_Penitipan_Abu.Master
{
    /// <summary>
    /// Interaction logic for MasterKategori.xaml
    /// </summary>
    public partial class MasterKategori : Window
    {
        public MasterKategori()
        {
            InitializeComponent();
            MySqlConnection conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("insert into kategori values(0,'test',2000,0)", conn);
            cmd.ExecuteNonQuery();
            
        }
    }
}
