using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using FinalProtingII.Helpers;

namespace FinalProtingII.Controllers
{
    public class JobdeskController : Controller
    {
        private static List<Karyawan> karyawans = new List<Karyawan>
        {
            new Karyawan { Id = 1, Nama = "Budi" },
            new Karyawan { Id = 2, Nama = "Sari" }
        };

        public IActionResult Index()
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            return View(jobdesks);
        }

        public IActionResult Create()
        {
            return PartialView("_FormCreate", new Jobdesk());
        }

        [HttpPost]
        public IActionResult Create(Jobdesk model, string TugasUtamaString)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            model.IdJobdesk = jobdesks.Count > 0 ? jobdesks.Max(j => j.IdJobdesk) + 1 : 1;

            if (!string.IsNullOrWhiteSpace(TugasUtamaString))
            {
                model.TugasUtama = TugasUtamaString
                    .Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();
            }

            JobdeskHelper.TambahJobdesk(model);
            return RedirectToAction("Index");
        }

        public IActionResult Assign()
        {
            ViewBag.Karyawans = karyawans;
            ViewBag.Jobdesks = JobdeskHelper.LoadJobdesk();
            return PartialView("_FormAssign");
        }

        [HttpPost]
        public IActionResult Assign(int jobdeskId, int karyawanId)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            var originalJobdesk = jobdesks.FirstOrDefault(j => j.IdJobdesk == jobdeskId);
            var karyawan = karyawans.FirstOrDefault(k => k.Id == karyawanId);

            if (originalJobdesk != null && karyawan != null)
            {
                // Clone jobdesk
                var newJobdesk = new Jobdesk
                {
                    IdJobdesk = jobdesks.Count > 0 ? jobdesks.Max(j => j.IdJobdesk) + 1 : 1,
                    NamaJobdesk = originalJobdesk.NamaJobdesk,
                    TugasUtama = new List<string>(originalJobdesk.TugasUtama),
                    KaryawanNama = karyawan.Nama
                };

                jobdesks.Add(newJobdesk);
                JobdeskHelper.SimpanJobdesk(jobdesks);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int jobdeskId, int karyawanId)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            var jobdesk = jobdesks.FirstOrDefault(j => j.IdJobdesk == jobdeskId);
            var karyawan = karyawans.FirstOrDefault(k => k.Id == karyawanId);

            if (jobdesk != null && karyawan != null)
            {
                jobdesk.KaryawanNama = karyawan.Nama;
                JobdeskHelper.SimpanJobdesk(jobdesks);
            }

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var jobdesk = JobdeskHelper.GetById(id);
            return PartialView("_FormDelete", jobdesk);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            JobdeskHelper.HapusJobdesk(id);
            return RedirectToAction("Index");
        }
    }
}
