using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikasi_Penitipan_Abu.Transaksi
{
    class Registrasi
    {
        public int idRegistrasi { get; set; }
        public int id_abu { get; set; }
        public int id_penanggung_jawab { get; set; }
        public int id_penanggung_jawab_2 { get; set; }
        public int id_kotak { get; set; }
        public string nama_abu { get; set; }
        public string nama_abu_alternatif { get; set; }
        public string nama_penanggung_jawab { get; set; }
        public string nama_penanggung_jawab_2 { get; set; }
        public int harga_kotak { get; set; }
        public string no_kotak { get; set; }

        public Registrasi()
        {
            idRegistrasi = -1;
            id_abu = -1;
            id_penanggung_jawab = -1;
            id_penanggung_jawab_2 = -1;
            id_kotak = -1;
        }
    }
}
