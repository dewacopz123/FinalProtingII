using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProtingII.Models
{
    public class Jobdesk
    {
        public int Id { get; set; }

        [Required]
        public int KaryawanId { get; set; }  // Foreign Key

        public Karyawan Karyawan { get; set; }  // Navigation Property

        [Required]
        public string Deskripsi { get; set; }

        public DateTime TanggalPekerjaan { get; set; }
    }
}
