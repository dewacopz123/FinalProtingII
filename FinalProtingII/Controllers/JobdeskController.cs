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

            // Mapping jobdesk ke karyawan
            var jobdeskToKaryawans = KaryawanJobdeskService.GetKaryawansForJobdesks(jobdesks.Select(j => j.IdJobdesk).ToList());

            ViewBag.JobdeskToKaryawans = jobdeskToKaryawans;

            // Dropdown data
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
            // Hapus semua assignment jobdesk ini, lalu tambah assignment baru
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
            // Cari jobdesk berdasarkan filter jobdeskName dan karyawanName
            var filteredJobdesks = KaryawanJobdeskService.SearchJobdesk(nama: jobdeskName);

            List<int> jobdeskIds = filteredJobdesks.Select(j => j.IdJobdesk).ToList();

            if (!string.IsNullOrWhiteSpace(karyawanName))
            {
                // Cari karyawan yang sesuai filter karyawanName
                var filteredKaryawans = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);
                var karyawanIds = filteredKaryawans.Select(k => k.Id).ToList();

                // Cari jobdesk yang terkait karyawan tsb
                var assignments = JobdeskAssignmentHelper.LoadAssignments();
                var jobdeskIdsByKaryawan = assignments
                    .Where(a => karyawanIds.Contains(a.KaryawanId))
                    .Select(a => a.JobdeskId)
                    .Distinct()
                    .ToList();

                // Intersect jobdesk hasil filter jobdeskName dan karyawanName
                jobdeskIds = jobdeskIds.Intersect(jobdeskIdsByKaryawan).ToList();

                filteredJobdesks = filteredJobdesks
                    .Where(j => jobdeskIds.Contains(j.IdJobdesk))
                    .ToList();
            }

            // Mapping jobdesk ke karyawan untuk jobdesk yang difilter
            var jobdeskToKaryawans = KaryawanJobdeskService.GetKaryawansForJobdesks(jobdeskIds);

            var allKaryawans = KaryawanHelper.LoadKaryawan();
            var allJobdesks = JobdeskHelper.LoadJobdesk();

            ViewBag.JobdeskNames = allJobdesks
                .Select(j => j.NamaJobdesk)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

            ViewBag.KaryawanNames = allKaryawans
                .Select(k => k.Nama)
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList();

            ViewBag.JobdeskToKaryawans = jobdeskToKaryawans;

            return View("Index", filteredJobdesks);
        }
    }
}
