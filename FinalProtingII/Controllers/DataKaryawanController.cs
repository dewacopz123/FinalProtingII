using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using System.Collections.Generic;
using System.Linq;

namespace FinalProtingII.Controllers
{
    public class DataKaryawanController : Controller
    {
        private static List<Karyawan> _karyawanList = new List<Karyawan>
        {
            new Karyawan { Id = 1, Nama = "Budi", Email = "budi@mail.com", Telepon = "081111", Role = Role.Karyawan, Status = 1 },
            new Karyawan { Id = 2, Nama = "Sari", Email = "sari@mail.com", Telepon = "082222", Role = Role.Karyawan, Status = 1 }
        };

        public IActionResult Index()
        {
            return View(_karyawanList);
        }

        public IActionResult Create()
        {
            return PartialView("_FormCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Karyawan karyawan)
        {
            karyawan.Id = _karyawanList.Any() ? _karyawanList.Max(k => k.Id) + 1 : 1;
            _karyawanList.Add(karyawan);
            return RedirectToAction("Index");
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var karyawan = _karyawanList.FirstOrDefault(k => k.Id == id);
            if (karyawan == null)
                return NotFound();

            return PartialView("_FormEdit", karyawan);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Karyawan updated)
        {
            var existing = _karyawanList.FirstOrDefault(k => k.Id == updated.Id);
            if (existing == null)
                return NotFound();

            existing.Nama = updated.Nama;
            existing.Email = updated.Email;
            existing.Telepon = updated.Telepon;
            existing.Role = updated.Role;
            existing.Status = updated.Status;

            return RedirectToAction("Index");
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var karyawan = _karyawanList.FirstOrDefault(k => k.Id == id);
            if (karyawan == null)
                return NotFound();

            return PartialView("_FormDelete", karyawan);
        }

        // POST: DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var karyawan = _karyawanList.FirstOrDefault(k => k.Id == id);
            if (karyawan != null)
                _karyawanList.Remove(karyawan);

            return RedirectToAction("Index");
        }
    }
}
