using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using FinalProtingII.Helpers;
using System.Linq;
using System.Collections.Generic;
using System;

namespace FinalProtingII.Controllers
{
    public class DataKaryawanController : Controller
    {
        // Generic helper method
        private List<T> LoadData<T>()
        {
            if (typeof(T) == typeof(Karyawan))
                return KaryawanHelper.LoadKaryawan() as List<T>;

            throw new NotSupportedException($"Tipe {typeof(T).Name} tidak didukung.");
        }

        private void SaveData<T>(List<T> data)
        {
            if (typeof(T) == typeof(Karyawan))
                KaryawanHelper.SimpanKaryawan(data as List<Karyawan>);
            else
                throw new NotSupportedException($"Tipe {typeof(T).Name} tidak didukung.");
        }

        public IActionResult Index(string karyawanName)
        {
            var allData = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);

            ViewBag.KaryawanNames = LoadData<Karyawan>()
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
            var list = LoadData<Karyawan>();
            karyawan.Id = list.Any() ? list.Max(k => k.Id) + 1 : 1;
            list.Add(karyawan);
            SaveData(list);
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
            var list = LoadData<Karyawan>();
            var existing = list.FirstOrDefault(k => k.Id == updated.Id);
            if (existing == null)
                return NotFound();

            var updateActions = new Dictionary<string, Action>
            {
                { "Nama", () => existing.Nama = updated.Nama },
                { "Email", () => existing.Email = updated.Email },
                { "Telepon", () => existing.Telepon = updated.Telepon },
                { "Role", () => existing.Role = updated.Role },
                { "Status", () => existing.Status = updated.Status }
            };

            foreach (var action in updateActions.Values)
            {
                action.Invoke();
            }

            SaveData(list);
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
            var list = LoadData<Karyawan>();
            var item = list.FirstOrDefault(k => k.Id == id);
            if (item != null)
            {
                list.Remove(item);
                SaveData(list);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string karyawanName)
        {
            var karyawanList = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);

            ViewBag.KaryawanNames = LoadData<Karyawan>()
                .Select(k => k.Nama)
                .Distinct()
                .ToList();

            return View("Index", karyawanList);
        }
    }
}
