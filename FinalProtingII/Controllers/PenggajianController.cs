using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using FinalProtingII.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace FinalProtingII.Controllers
{
    public class PenggajianController : Controller
    {
        // Menampilkan daftar penggajian, dengan opsi pencarian berdasarkan nama karyawan
        public IActionResult Index(string karyawanName)
        {
            var penggajianList = PenggajianHelper.LoadPenggajian();
            var karyawanList = KaryawanHelper.LoadKaryawan();

            // Membuat dictionary untuk mengambil nama karyawan berdasarkan Id
            var karyawanDict = karyawanList.ToDictionary(k => k.Id, k => k.Nama);
            ViewBag.KaryawanDict = karyawanDict;

            // Filter penggajian jika nama karyawan dicari
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

            // Mengirim daftar nama karyawan unik untuk kebutuhan dropdown/filter di view
            ViewBag.KaryawanNames = karyawanList
                .Select(k => k.Nama)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

            return View(penggajianList);
        }

        // Mengembalikan status penggajian selanjutnya berdasarkan status saat ini
        public PenggajianStatus GetNextStatus(PenggajianStatus current)
        {
            return current switch
            {
                PenggajianStatus.Draft => PenggajianStatus.Submitted,
                PenggajianStatus.Submitted => PenggajianStatus.Approved,
                PenggajianStatus.Approved => PenggajianStatus.Paid,
                _ => current
            };
        }

        // Proses untuk memperbarui status penggajian ke status berikutnya
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvanceStatus(int id)
        {
            var list = PenggajianHelper.LoadPenggajian();
            var penggajian = list.FirstOrDefault(p => p.Id == id);
            if (penggajian == null) return NotFound();

            penggajian.Status = GetNextStatus(penggajian.Status);
            PenggajianHelper.SavePenggajian(list);

            return RedirectToAction("Index");
        }

        // Menampilkan form create sebagai partial view
        public IActionResult Create()
        {
            ViewBag.KaryawanList = KaryawanHelper.LoadKaryawan();
            return PartialView("_FormCreate");
        }

        // Menangani proses pembuatan data penggajian baru
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
            if (penggajian == null)
                return NotFound();

            ViewBag.KaryawanList = KaryawanHelper.LoadKaryawan();
            return PartialView("_FormEdit", penggajian);
        }

        // Menyimpan perubahan data penggajian yang telah diedit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Penggajian model)
        {
            var penggajianList = PenggajianHelper.LoadPenggajian();
            var existing = penggajianList.FirstOrDefault(p => p.Id == model.Id);

            if (existing == null)
                return NotFound();

            // Update data
            existing.IdKaryawan = model.IdKaryawan;
            existing.Tanggal = model.Tanggal;
            existing.GajiPokok = model.GajiPokok;

            PenggajianHelper.SavePenggajian(penggajianList);

            return RedirectToAction("Index");
        }


        // Menampilkan form konfirmasi penghapusan penggajian
        public IActionResult Delete(int id)
        {
            var penggajian = PenggajianHelper.LoadPenggajian().FirstOrDefault(p => p.Id == id);
            var karyawan = KaryawanHelper.LoadKaryawan().FirstOrDefault(k => k.Id == penggajian?.IdKaryawan);

            ViewBag.NamaKaryawan = karyawan?.Nama ?? "Tidak Diketahui";
            return PartialView("_FormDelete", penggajian);
        }

        // Menangani konfirmasi dan proses penghapusan data penggajian
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

        // Mengalihkan pencarian ke halaman index agar tidak duplikatif
        [HttpGet]
        public IActionResult Search(string karyawanName)
        {
            return RedirectToAction("Index", new { karyawanName });
        }
    }
}
