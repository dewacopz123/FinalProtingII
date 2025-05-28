using FinalProtingII.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FinalProtingII.Controllers
{
    public class PenggajianController : Controller
    {
        private static List<Penggajian> data = new List<Penggajian>
        {
            new Penggajian { Id = 1, NamaKaryawan = "Budi", Tanggal = DateTime.Today, GajiPokok = 5000000 },
            new Penggajian { Id = 2, NamaKaryawan = "Sari", Tanggal = DateTime.Today, GajiPokok = 4500000 }
        };

        public IActionResult Index()
        {
            return View(data);
        }

        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        public IActionResult Edit(int id)
        {
            return PartialView("_Edit");
        }

        public IActionResult Delete(int id)
        {
            return PartialView("_Delete");
        }
    }
}
