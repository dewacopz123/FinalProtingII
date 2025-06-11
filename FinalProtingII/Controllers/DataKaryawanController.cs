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

        private delegate IActionResult KaryawanActionDelegate(object data);

        private static readonly Dictionary<string, KaryawanActionDelegate> _actions = new Dictionary<string, KaryawanActionDelegate>
        {
            { "_FormCreate", data => {
                var karyawan = data as Karyawan;
                karyawan.Id = _karyawanList.Any() ? _karyawanList.Max(k => k.Id) + 1 : 1;
                _karyawanList.Add(karyawan);
                return new RedirectToActionResult("Index", "DataKaryawan", null);
            }},
            { "_FromEdit", data => {
                var updated = data as Karyawan;
                var existing = _karyawanList.FirstOrDefault(k => k.Id == updated.Id);
                if (existing == null) return new NotFoundResult();

                existing.Nama = updated.Nama;
                existing.Email = updated.Email;
                existing.Telepon = updated.Telepon;
                existing.Role = updated.Role;
                existing.Status = updated.Status;
                return new RedirectToActionResult("Index", "DataKaryawan", null);
            }},
            { "_FormDelete", data => {
                int id = (int)data;
                var karyawan = _karyawanList.FirstOrDefault(k => k.Id == id);
                if (karyawan != null)
                    _karyawanList.Remove(karyawan);
                return new RedirectToActionResult("Index", "DataKaryawan", null);
            }}
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
            return _actions["_FormCreate"](karyawan);
        }

        public IActionResult Edit(int id)
        {
            var karyawan = _karyawanList.FirstOrDefault(k => k.Id == id);
            if (karyawan == null)
                return NotFound();

            return PartialView("_FormEdit", karyawan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Karyawan updated)
        {
            return _actions["_FormEdit"](updated);
        }

        public IActionResult Delete(int id)
        {
            var karyawan = _karyawanList.FirstOrDefault(k => k.Id == id);
            if (karyawan == null)
                return NotFound();

            return PartialView("_FormDelete", karyawan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return _actions["_FormDelete"](id);
        }
    }
}
