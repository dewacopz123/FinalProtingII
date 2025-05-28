using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using System.Collections.Generic;

namespace FinalProtingII.Controllers
{
    public class DataKaryawanController : Controller
    {
        public IActionResult Index()
        {
            var karyawanList = new List<Karyawan>()
            {
                new Karyawan { Id = 1, Nama = "Budi" },
                new Karyawan { Id = 2, Nama = "Sari" }
            };
            return View(karyawanList);
        }
    }
}
