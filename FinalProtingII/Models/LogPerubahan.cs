namespace AplikasiAbsensi.Core.Models
{
    public class LogPerubahan
    {
        public DateTime Waktu { get; set; }
        public string Aksi { get; set; }
        public string NamaKaryawan { get; set; }
        public int IdKaryawan { get; set; }
        public string Detail { get; set; }
    }
}
