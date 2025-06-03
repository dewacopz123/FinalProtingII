using FinalProtingII.Models;
using Microsoft.AspNetCore.Mvc;
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
            return PartialView("Create", new Penggajian());
        }

        [HttpPost]
        public IActionResult Create(Penggajian penggajian)
        {
            penggajian.Id = data.Any() ? data.Max(x => x.Id) + 1 : 1;
            data.Add(penggajian);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var item = data.FirstOrDefault(x => x.Id == id);
            return PartialView("Edit", item);
        }

        [HttpPost]
        public IActionResult Edit(Penggajian penggajian)
        {
            var existing = data.FirstOrDefault(x => x.Id == penggajian.Id);
            if (existing != null)
            {
                existing.NamaKaryawan = penggajian.NamaKaryawan;
                existing.Tanggal = penggajian.Tanggal;
                existing.GajiPokok = penggajian.GajiPokok;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var item = data.FirstOrDefault(x => x.Id == id);
            return PartialView("Delete", item);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = data.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                data.Remove(item);
            }
            return RedirectToAction("Index");
        }
    }
}
