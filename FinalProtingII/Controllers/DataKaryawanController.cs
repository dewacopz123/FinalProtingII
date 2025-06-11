using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using FinalProtingII.Helpers;
using System.Linq;

namespace FinalProtingII.Controllers
{
    public class DataKaryawanController : Controller
    {
        public IActionResult Index(string karyawanName)
        {
            // Pakai service pencarian reusable
            var allData = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);

            ViewBag.KaryawanNames = KaryawanHelper.LoadKaryawan()
                .Select(k => k.Nama)
                .Distinct()
                .ToList();

            return View(allData);
        }

        public IActionResult Create()
        {
            return PartialView("_FormCreate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Karyawan karyawan)
        {
            var list = KaryawanHelper.LoadKaryawan();
            karyawan.Id = list.Any() ? list.Max(k => k.Id) + 1 : 1;
            list.Add(karyawan);
            KaryawanHelper.SimpanKaryawan(list);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var karyawan = KaryawanHelper.GetById(id);
            if (karyawan == null)
                return NotFound();

            return PartialView("_FormEdit", karyawan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Karyawan updated)
        {
            var list = KaryawanHelper.LoadKaryawan();
            var existing = list.FirstOrDefault(k => k.Id == updated.Id);
            if (existing == null)
                return NotFound();

            existing.Nama = updated.Nama;
            existing.Email = updated.Email;
            existing.Telepon = updated.Telepon;
            existing.Role = updated.Role;
            existing.Status = updated.Status;

            KaryawanHelper.SimpanKaryawan(list);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var karyawan = KaryawanHelper.GetById(id);
            if (karyawan == null)
                return NotFound();

            return PartialView("_FormDelete", karyawan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var list = KaryawanHelper.LoadKaryawan();
            var item = list.FirstOrDefault(k => k.Id == id);
            if (item != null)
            {
                list.Remove(item);
                KaryawanHelper.SimpanKaryawan(list);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string karyawanName)
        {
            var karyawanList = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);

            ViewBag.KaryawanNames = KaryawanHelper.LoadKaryawan()
                .Select(k => k.Nama)
                .Distinct()
                .ToList();

            return View("Index", karyawanList);
        }
    }
}
