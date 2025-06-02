using System.Collections.Generic;

namespace AplikasiAbsensi.Core.Models
{
    public class JobDesk
    {
        public int IdJobdesk { get; set; }
        public string NamaJobdesk { get; set; }
        public List<string> TugasUtama { get; set; } = new List<string>();

        public JobDesk() { }

        public JobDesk(int id, string nama, List<string> tugas)
        {
            IdJobdesk = id;
            NamaJobdesk = nama;
            TugasUtama = tugas ?? new List<string>();
        }

        public override string ToString()
        {
            return $"{NamaJobdesk} ({string.Join(", ", TugasUtama)})";
        }
    }
}
