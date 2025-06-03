using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using System.Collections.Generic;
using System.Linq;

namespace FinalProtingII.Controllers
{
    public class JobdeskController : Controller
    {
        private static List<Jobdesk> jobdesks = new List<Jobdesk>
        {
            new Jobdesk { IdJobdesk = 1, NamaJobdesk = "Laporan Harian", TugasUtama = new List<string>{ "Menulis laporan", "Kompilasi data" }, KaryawanNama = "Budi" },
            new Jobdesk { IdJobdesk = 2, NamaJobdesk = "Analisis Data", TugasUtama = new List<string>{ "Analisis Excel", "Visualisasi PowerBI" }, KaryawanNama = "Sari" }
        };

        private static List<Karyawan> karyawans = new List<Karyawan>
        {
            new Karyawan { Id = 1, Nama = "Budi" },
            new Karyawan { Id = 2, Nama = "Sari" }
        };

        public IActionResult Index()
        {
            return View(jobdesks);
        }

        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public IActionResult Create(Jobdesk model, string TugasUtamaString)
        {
            model.IdJobdesk = jobdesks.Count + 1;

            if (!string.IsNullOrWhiteSpace(TugasUtamaString))
            {
                model.TugasUtama = TugasUtamaString
                    .Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();
            }

            jobdesks.Add(model);
            return RedirectToAction("Index");
        }

        public IActionResult Assign()
        {
            ViewBag.Karyawans = karyawans;
            ViewBag.Jobdesks = jobdesks;
            return PartialView("_Assign");
        }

        [HttpPost]
        public IActionResult Assign(int jobdeskId, int karyawanId)
        {
            var jobdesk = jobdesks.FirstOrDefault(j => j.IdJobdesk == jobdeskId);
            var karyawan = karyawans.FirstOrDefault(k => k.Id == karyawanId);

            if (jobdesk != null && karyawan != null)
            {
                jobdesk.KaryawanNama = karyawan.Nama;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var jobdesk = jobdesks.FirstOrDefault(j => j.IdJobdesk == id);
            return PartialView("_Delete", jobdesk);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var jobdesk = jobdesks.FirstOrDefault(j => j.IdJobdesk == id);
            if (jobdesk != null)
            {
                jobdesks.Remove(jobdesk);
            }
            return RedirectToAction("Index");
        }
    }
}
