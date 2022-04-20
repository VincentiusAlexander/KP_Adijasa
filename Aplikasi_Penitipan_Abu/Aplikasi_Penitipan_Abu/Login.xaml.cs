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
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace Aplikasi_Penitipan_Abu
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        MySqlConnection conn;
        public Window1()
        {
            InitializeComponent();
            conn = new MySqlConnection("datasource=localhost; port=3306; username=root; password=;database=penitipan_abu_adijasa");
        }

        public void login()
        {
            string password = getMD5Hash(txt_password.Password);
            MySqlCommand cmd = new MySqlCommand("select * from users where username = ?username and password = ?password", conn);
            cmd.Parameters.AddWithValue("?username", txt_username.Text);
            cmd.Parameters.AddWithValue("?password", password);
            conn.Close();
            conn.Open();
            Boolean isUser = false;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) if (reader.GetValue(2).ToString().Equals(password)) isUser = true;
            if (isUser)
            {
                Menu overview = new Menu(reader.GetInt32(3));
                overview.Show();
                conn.Close();
                this.Close();
                return;
            }
            conn.Close();
            System.Windows.Forms.MessageBox.Show("Gagal Login, periksa kembali username dan password", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }

        public string getMD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            login();
        }
    }
}
