using System.ComponentModel.DataAnnotations;

namespace FinalProtingII.Models
{
    public class Karyawan
    {
        public int Id { get; set; }

        [Required]
        public string Nama { get; set; }
    }
}
