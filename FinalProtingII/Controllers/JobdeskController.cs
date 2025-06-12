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

            var jobdeskToKaryawans = KaryawanJobdeskService.GetKaryawansForJobdesks(
                jobdesks.Select(j => j.IdJobdesk).ToList());

            ViewBag.JobdeskToKaryawans = jobdeskToKaryawans;
            ViewBag.JobdeskNames = jobdesks.Select(j => j.NamaJobdesk).Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();
            ViewBag.KaryawanNames = allKaryawans.Select(k => k.Nama).Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();

            return View(jobdesks);
        }

        public IActionResult Create() => PartialView("_FormCreate", new Jobdesk());

        [HttpPost]
        public IActionResult Create(Jobdesk model, string TugasUtamaString)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            model.IdJobdesk = jobdesks.Count > 0 ? jobdesks.Max(j => j.IdJobdesk) + 1 : 1;

            if (!string.IsNullOrWhiteSpace(TugasUtamaString))
            {
                model.TugasUtama = TugasUtamaString.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();
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
            if (jobdeskId == 0 || karyawanId == 0)
            {
                TempData["Error"] = "Silakan pilih Jobdesk dan Karyawan.";
                return RedirectToAction("Index");
            }

            var assignment = new JobdeskAssignment { JobdeskId = jobdeskId, KaryawanId = karyawanId };
            JobdeskAssignmentHelper.TambahAssignment(assignment);

            TempData["Success"] = "Jobdesk berhasil diberikan ke karyawan.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reassign(int jobdeskId, int karyawanId)
        {
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
            var filteredJobdesks = KaryawanJobdeskService.SearchJobdesk(nama: jobdeskName);
            var jobdeskIds = filteredJobdesks.Select(j => j.IdJobdesk).ToList();

            if (!string.IsNullOrWhiteSpace(karyawanName))
            {
                var filteredKaryawans = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);
                var karyawanIds = filteredKaryawans.Select(k => k.Id).ToList();

                var assignments = JobdeskAssignmentHelper.LoadAssignments();
                var jobdeskIdsByKaryawan = assignments
                    .Where(a => karyawanIds.Contains(a.KaryawanId))
                    .Select(a => a.JobdeskId)
                    .Distinct()
                    .ToList();

                jobdeskIds = jobdeskIds.Intersect(jobdeskIdsByKaryawan).ToList();

                filteredJobdesks = filteredJobdesks
                    .Where(j => jobdeskIds.Contains(j.IdJobdesk))
                    .ToList();
            }

            var jobdeskToKaryawans = KaryawanJobdeskService.GetKaryawansForJobdesks(jobdeskIds);
            var allKaryawans = KaryawanHelper.LoadKaryawan();
            var allJobdesks = JobdeskHelper.LoadJobdesk();

            ViewBag.JobdeskNames = allJobdesks.Select(j => j.NamaJobdesk).Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();
            ViewBag.KaryawanNames = allKaryawans.Select(k => k.Nama).Where(n => !string.IsNullOrEmpty(n)).Distinct().ToList();
            ViewBag.JobdeskToKaryawans = jobdeskToKaryawans;

            return View("Index", filteredJobdesks);
        }

        // Untuk GET
        [HttpGet("Jobdesk/Edit/{id}")]
        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
            var jobdesk = JobdeskHelper.GetById(id);
            return PartialView("_FormEdit", jobdesk);
        }



        // POST: Save changes to jobdesk
        [HttpPost("Jobdesk/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Jobdesk model, string TugasUtamaString)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            var existingJobdesk = jobdesks.FirstOrDefault(j => j.IdJobdesk == id);
            if (existingJobdesk == null)
            {
                return NotFound();
            }

            existingJobdesk.NamaJobdesk = model.NamaJobdesk;

            if (!string.IsNullOrWhiteSpace(TugasUtamaString))
            {
                existingJobdesk.TugasUtama = TugasUtamaString
                    .Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();
            }

            JobdeskHelper.SimpanJobdesk(jobdesks);
            return RedirectToAction("Index");
        }

    }
}
