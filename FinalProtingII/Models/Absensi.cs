namespace FinalProtingII.Models
{
    public class Absensi
    {
        public int Id { get; set; }
        public string NamaKaryawan { get; set; }
        public DateTime Tanggal { get; set; }
        public string Status { get; set; } // "Masuk" atau "Selesai"
    }

}
