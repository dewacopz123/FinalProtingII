﻿using System.ComponentModel.DataAnnotations;

namespace FinalProtingII.Models
{
    public enum Role
    {
        Karyawan
    }

    public class Karyawan
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Telepon { get; set; }
        public Role Role { get; set; }
        public int Status { get; set; }

    }
}
