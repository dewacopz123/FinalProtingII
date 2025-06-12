using System;

namespace FinalProtingII.Models
{
    public enum PenggajianStatus
    {
        Draft,
        Submitted,
        Approved,
        Paid
    }

    public class Penggajian
    {
        public int Id { get; set; }

        public int IdKaryawan { get; set; }  // Pastikan properti ini sesuai dengan semua penggunaan
        public string NamaKaryawan { get; set; }

        public DateTime Tanggal { get; set; }

        public decimal GajiPokok { get; set; }

        public Karyawan Karyawan { get; set; }  // untuk relasi (optional kalau pakai JSON)

        public PenggajianStatus Status { get; set; } = PenggajianStatus.Draft;
    }
}
