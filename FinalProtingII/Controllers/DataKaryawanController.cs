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
        // Table-driven dictionaries for Load and Save methods
        private static readonly Dictionary<Type, Func<object>> LoadTable = new()
        {
            { typeof(Karyawan), () => KaryawanHelper.LoadKaryawan() }
        };

        private static readonly Dictionary<Type, Action<object>> SaveTable = new()
        {
            { typeof(Karyawan), data => KaryawanHelper.SimpanKaryawan((List<Karyawan>)data) }
        };

        // Generic Load method using table-driven approach
        private List<T> LoadData<T>()
        {
            if (LoadTable.TryGetValue(typeof(T), out var loader))
            {
                return loader() as List<T>;
            }
            throw new NotSupportedException($"Tipe {typeof(T).Name} tidak didukung.");
        }

        // Generic Save method using table-driven approach
        private void SaveData<T>(List<T> data)
        {
            if (SaveTable.TryGetValue(typeof(T), out var saver))
            {
                saver(data);
            }
            else
            {
                throw new NotSupportedException($"Tipe {typeof(T).Name} tidak didukung.");
            }
        }

        private void PopulateKaryawanNames()
        {
            ViewBag.KaryawanNames = LoadData<Karyawan>()
                .Select(k => k.Nama)
                .Distinct()
                .ToList();
        }

        public IActionResult Index(string karyawanName)
        {
            var result = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);
            PopulateKaryawanNames();

            return View(result);
        }

        public IActionResult Create() => PartialView("_FormCreate");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Karyawan karyawan)
        {
            var list = LoadData<Karyawan>();
            karyawan.Id = list.Any() ? list.Max(k => k.Id) + 1 : 1;

            list.Add(karyawan);
            SaveData(list);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var karyawan = KaryawanHelper.GetById(id);

            return karyawan is null ? NotFound() : PartialView("_FormEdit", karyawan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Karyawan updated)
        {
            var list = LoadData<Karyawan>();
            var existing = list.FirstOrDefault(k => k.Id == updated.Id);
            if (existing == null) return NotFound();

            UpdateKaryawan(existing, updated);
            SaveData(list);

            return RedirectToAction(nameof(Index));
        }

        private static readonly Dictionary<string, Action<Karyawan, Karyawan>> UpdateFieldMap = new()
        {
            { "Nama", (existing, updated) => existing.Nama = updated.Nama },
            { "Email", (existing, updated) => existing.Email = updated.Email },
            { "Telepon", (existing, updated) => existing.Telepon = updated.Telepon },
            { "Role", (existing, updated) => existing.Role = updated.Role },
            { "Status", (existing, updated) => existing.Status = updated.Status }
        };

        private void UpdateKaryawan(Karyawan existing, Karyawan updated)
        {
            foreach (var updateField in UpdateFieldMap.Values)
            {
                updateField(existing, updated);
            }
        }

        public IActionResult Delete(int id)
        {
            var karyawan = KaryawanHelper.GetById(id);

            return karyawan is null ? NotFound() : PartialView("_FormDelete", karyawan);
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

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Search(string karyawanName)
        {
            var results = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);
            PopulateKaryawanNames();

            return View("Index", results);
        }
    }
}
