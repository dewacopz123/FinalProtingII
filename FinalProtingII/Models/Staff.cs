using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikasiAbsensi.Core.Models
{
    public class Staff
    {
        public int Id_Staff { get; set; }
        public string Nama_Staff { get; set; }
        public string Email_Staff { get; set; }
        public string Phone_Staff { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }


        public Staff(int id_Staff, string nama_Staff, string email_Staff, string phone_Staff, int role, int status)
        {
            Id_Staff = id_Staff;
            Nama_Staff = nama_Staff;
            Email_Staff = email_Staff;
            Phone_Staff = phone_Staff;
            Role = role;
            Status = status;
        }
    }
}
