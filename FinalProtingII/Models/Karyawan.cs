using System;

namespace AplikasiAbsensi.Core.Models
{
    public class Karyawan
    {
        public int Id_Karyawan { get; set; }
        public string Nama_Karyawan { get; set; }
        public string Email_Karyawan { get; set; }
        public string Phone_Karyawan { get; set; }

        public Role Role { get; set; } 
        public int Status { get; set; }

        public int Gaji { get; set; }

        public int JobdeskId { get; set; }
        public JobDesk Jobdesk { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public DateTime Waktu { get; set; }
        public string Tipe { get; set; }

        public Karyawan(int id_Karyawan, string nama_Karyawan, string email_Karyawan, string phone_Karyawan, Role role, int status, int gaji, int jobdeskId)
        {
            Id_Karyawan = id_Karyawan;
            Nama_Karyawan = nama_Karyawan;
            Email_Karyawan = email_Karyawan;
            Phone_Karyawan = phone_Karyawan;
            Role = role;
            Status = status;
            Gaji = gaji;
            JobdeskId = jobdeskId;
            Jobdesk = null;
            CheckInTime = null;
            CheckOutTime = null;
            Waktu = DateTime.Now;
            Tipe = null;
        }
    }
}
