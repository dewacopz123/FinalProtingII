using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using FinalProtingII.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace FinalProtingII.Controllers
{
    public class PenggajianController : Controller
    {
        public IActionResult Index(string karyawanName)
        {
            var penggajianList = PenggajianHelper.LoadPenggajian();
            var karyawanList = KaryawanHelper.LoadKaryawan();

            // Buat dictionary untuk ambil nama karyawan berdasarkan IdKaryawan
            var karyawanDict = karyawanList.ToDictionary(k => k.Id, k => k.Nama);
            ViewBag.KaryawanDict = karyawanDict;

            // Filter penggajianList jika karyawanName diisi
            if (!string.IsNullOrEmpty(karyawanName))
            {
                var matchingIds = karyawanList
                    .Where(k => k.Nama != null && k.Nama.Contains(karyawanName, System.StringComparison.OrdinalIgnoreCase))
                    .Select(k => k.Id)
                    .ToHashSet();

                penggajianList = penggajianList
                    .Where(p => matchingIds.Contains(p.IdKaryawan))
                    .ToList();
            }

            ViewBag.KaryawanNames = karyawanList
                .Select(k => k.Nama)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

            return View(penggajianList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvanceStatus(int id)
        {
            var list = PenggajianHelper.LoadPenggajian();
            var penggajian = list.FirstOrDefault(p => p.Id == id);
            if (penggajian == null) return NotFound();

            PenggajianHelper.SavePenggajian(list);
            return RedirectToAction("Index");
        }


        public IActionResult Create()
        {
            ViewBag.KaryawanList = KaryawanHelper.LoadKaryawan();
            return PartialView("_FormCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Penggajian penggajian)
        {
            var list = PenggajianHelper.LoadPenggajian();
            penggajian.Id = list.Any() ? list.Max(p => p.Id) + 1 : 1;
            list.Add(penggajian);
            PenggajianHelper.SavePenggajian(list);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var penggajian = PenggajianHelper.LoadPenggajian().FirstOrDefault(p => p.Id == id);
            if (penggajian == null) return NotFound();

            ViewBag.KaryawanList = KaryawanHelper.LoadKaryawan();
            return PartialView("_FormEdit", penggajian);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Penggajian updated)
        {
            var list = PenggajianHelper.LoadPenggajian();
            var existing = list.FirstOrDefault(p => p.Id == updated.Id);
            if (existing == null) return NotFound();

            existing.IdKaryawan = updated.IdKaryawan;
            existing.Tanggal = updated.Tanggal;
            existing.GajiPokok = updated.GajiPokok;

            PenggajianHelper.SavePenggajian(list);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var penggajian = PenggajianHelper.LoadPenggajian().FirstOrDefault(p => p.Id == id);
            var karyawan = KaryawanHelper.LoadKaryawan().FirstOrDefault(k => k.Id == penggajian?.IdKaryawan);
            ViewBag.NamaKaryawan = karyawan?.Nama ?? "Tidak Diketahui";
            return PartialView("_FormDelete", penggajian);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var list = PenggajianHelper.LoadPenggajian();
            var item = list.FirstOrDefault(p => p.Id == id);
            if (item != null)
            {
                list.Remove(item);
                PenggajianHelper.SavePenggajian(list);
            }
            return RedirectToAction("Index");
        }

        // Hapus method Search atau redirect ke Index
        [HttpGet]
        public IActionResult Search(string karyawanName)
        {
            return RedirectToAction("Index", new { karyawanName });
        }

    }
}
