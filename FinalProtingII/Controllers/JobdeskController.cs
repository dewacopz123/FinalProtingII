using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using FinalProtingII.Helpers;

namespace FinalProtingII.Controllers
{
    public class JobdeskController : Controller
    {
        public IActionResult Index()
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            var assignments = JobdeskAssignmentHelper.LoadAssignments();
            var allKaryawans = KaryawanHelper.LoadKaryawan();

            Dictionary<int, List<string>> jobdeskToKaryawans = new();

            foreach (var jd in jobdesks)
            {
                var assignedIds = assignments
                    .Where(a => a.JobdeskId == jd.IdJobdesk)
                    .Select(a => a.KaryawanId)
                    .ToList();

                var names = allKaryawans
                    .Where(k => assignedIds.Contains(k.Id))
                    .Select(k => k.Nama)
                    .ToList();

                jobdeskToKaryawans[jd.IdJobdesk] = names;
            }

            ViewBag.JobdeskToKaryawans = jobdeskToKaryawans;

            // ✅ Tambahkan ViewBag untuk dropdown
            ViewBag.JobdeskNames = jobdesks
                .Select(j => j.NamaJobdesk)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

            ViewBag.KaryawanNames = allKaryawans
                .Select(k => k.Nama)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

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
            ViewBag.Karyawans = KaryawanHelper.LoadKaryawan();
            ViewBag.Jobdesks = JobdeskHelper.LoadJobdesk();
            return PartialView("_FormAssign");
        }

        [HttpPost]
        public IActionResult Assign(int jobdeskId, int karyawanId)
        {
            JobdeskAssignmentHelper.TambahAssignment(new JobdeskAssignment
            {
                JobdeskId = jobdeskId,
                KaryawanId = karyawanId
            });

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(int jobdeskId, int karyawanId)
        {
            // Hapus semua assignment jobdesk ini, lalu tambahkan baru
            JobdeskAssignmentHelper.HapusAssignmentsByJobdesk(jobdeskId);

            JobdeskAssignmentHelper.TambahAssignment(new JobdeskAssignment
            {
                JobdeskId = jobdeskId,
                KaryawanId = karyawanId
            });

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
            JobdeskAssignmentHelper.HapusAssignmentsByJobdesk(id);
            JobdeskHelper.HapusJobdesk(id);
            return RedirectToAction("Index");
        }

        public IActionResult Search(string jobdeskName, string karyawanName)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            var assignments = JobdeskAssignmentHelper.LoadAssignments();
            var allKaryawans = KaryawanHelper.LoadKaryawan();

            if (!string.IsNullOrWhiteSpace(jobdeskName))
            {
                jobdesks = jobdesks
                    .Where(j => !string.IsNullOrEmpty(j.NamaJobdesk) && j.NamaJobdesk.Contains(jobdeskName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(karyawanName))
            {
                var matchingKaryawanIds = allKaryawans
                    .Where(k => k.Nama.Contains(karyawanName, StringComparison.OrdinalIgnoreCase))
                    .Select(k => k.Id)
                    .ToList();

                var matchingJobdeskIds = assignments
                    .Where(a => matchingKaryawanIds.Contains(a.KaryawanId))
                    .Select(a => a.JobdeskId)
                    .Distinct()
                    .ToList();

                jobdesks = jobdesks
                    .Where(j => matchingJobdeskIds.Contains(j.IdJobdesk))
                    .ToList();
            }

            // Untuk dropdown
            ViewBag.JobdeskNames = JobdeskHelper.LoadJobdesk()
                .Select(j => j.NamaJobdesk)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

            ViewBag.KaryawanNames = allKaryawans
                .Select(k => k.Nama)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

            // Data mapping karyawan per jobdesk
            Dictionary<int, List<string>> jobdeskToKaryawans = new();

            foreach (var jd in jobdesks)
            {
                var assignedIds = assignments
                    .Where(a => a.JobdeskId == jd.IdJobdesk)
                    .Select(a => a.KaryawanId)
                    .ToList();

                var names = allKaryawans
                    .Where(k => assignedIds.Contains(k.Id))
                    .Select(k => k.Nama)
                    .ToList();

                jobdeskToKaryawans[jd.IdJobdesk] = names;
            }

            ViewBag.JobdeskToKaryawans = jobdeskToKaryawans;

            return View("Index", jobdesks);
        }
    }
}
