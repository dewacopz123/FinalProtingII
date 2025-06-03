using System;
using System.Collections.Generic;

namespace FinalProtingII.Models
{
    public class Jobdesk
    {
        public int IdJobdesk { get; set; }
        public string NamaJobdesk { get; set; }
        public List<string> TugasUtama { get; set; } = new List<string>();
        public string KaryawanNama { get; set; } // nama karyawan yang ditugaskan
    }
}
