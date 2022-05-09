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
using System.Windows.Markup;

namespace Aplikasi_Penitipan_Abu.Transaksi.PenitipanAbu
{
    /// <summary>
    /// Interaction logic for PenitipanAbu_FormFix.xaml
    /// </summary>
    public partial class PenitipanAbu_FormFix : Window
    {
        public PenitipanAbu_FormFix()
        {
            InitializeComponent();
        }

        private void btn_print_penitipan_abu_form_Click(object sender, RoutedEventArgs e)
        {
            btn_print_penitipan_abu_form.Visibility = Visibility.Hidden;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(grid, "Form Penitipan Abu");
            }
            btn_print_penitipan_abu_form.Visibility = Visibility.Visible;
        }
    }
}
