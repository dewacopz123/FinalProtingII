using AplikasiAbsensi.Core.Helper;
using AplikasiAbsensi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using System.Collections.Generic;
using System;

namespace FinalProtingII.Controllers
{
    public class JobdeskController : Controller
    {
        public IActionResult Index()
        {
            // Dummy data Karyawan
            var karyawan1 = new Karyawan { Id = 1, Nama = "Budi" };
            var karyawan2 = new Karyawan { Id = 2, Nama = "Siti" };

            var jobdeskList = new List<Jobdesk>()
            {
                new Jobdesk
                {
                    Id = 1,
                    Deskripsi = "Membuat laporan mingguan",
                    TanggalPekerjaan = DateTime.Now,
                    Karyawan = karyawan1
                },
                new Jobdesk
                {
                    Id = 2,
                    Deskripsi = "Diskusi kebutuhan project",
                    TanggalPekerjaan = DateTime.Now.AddDays(-1),
                    Karyawan = karyawan2
                }
            };

            return View(jobdeskList);
        }
    }
}
