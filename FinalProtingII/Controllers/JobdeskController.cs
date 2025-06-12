using Microsoft.AspNetCore.Mvc;
using FinalProtingII.Models;
using FinalProtingII.Helpers;

namespace FinalProtingII.Controllers
{
    public class JobdeskController : Controller
    {
        // Tampilkan seluruh jobdesk beserta relasi ke karyawan
        public IActionResult Index()
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            var assignments = JobdeskAssignmentHelper.LoadAssignments();
            var allKaryawans = KaryawanHelper.LoadKaryawan();

            var jobdeskToKaryawans = KaryawanJobdeskService.GetKaryawansForJobdesks(
                jobdesks.Select(j => j.IdJobdesk).ToList());

            ViewBag.JobdeskToKaryawans = jobdeskToKaryawans;
            ViewBag.JobdeskNames = jobdesks.Select(j => j.NamaJobdesk).WhereNotNullOrEmpty().Distinct().ToList();
            ViewBag.KaryawanNames = allKaryawans.Select(k => k.Nama).WhereNotNullOrEmpty().Distinct().ToList();

            return View(jobdesks);
        }

        public IActionResult Create() => PartialView("_FormCreate", new Jobdesk());

        [HttpPost]
        public IActionResult Create(Jobdesk model, string tugasUtama)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            model.IdJobdesk = jobdesks.Any() ? jobdesks.Max(j => j.IdJobdesk) + 1 : 1;

            model.TugasUtama = ParseTugasUtama(tugasUtama);

            JobdeskHelper.TambahJobdesk(model);

            return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }

            var assignment = new JobdeskAssignment { JobdeskId = jobdeskId, KaryawanId = karyawanId };
            JobdeskAssignmentHelper.TambahAssignment(assignment);

            TempData["Success"] = "Jobdesk berhasil diberikan ke karyawan.";
            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
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
                filteredJobdesks = filteredJobdesks.Where(j => jobdeskIds.Contains(j.IdJobdesk)).ToList();
            }

            ViewBag.JobdeskToKaryawans = KaryawanJobdeskService.GetKaryawansForJobdesks(jobdeskIds);
            ViewBag.JobdeskNames = JobdeskHelper.LoadJobdesk().Select(j => j.NamaJobdesk).WhereNotNullOrEmpty().Distinct().ToList();
            ViewBag.KaryawanNames = KaryawanHelper.LoadKaryawan().Select(k => k.Nama).WhereNotNullOrEmpty().Distinct().ToList();

            return View("Index", filteredJobdesks);
        }

        [HttpGet("Jobdesk/Edit/{id}")]
        [ActionName("Edit")]
        public IActionResult EditGet(int id)
        {
            var jobdesk = JobdeskHelper.GetById(id);
            return PartialView("_FormEdit", jobdesk);
        }

        [HttpPost("Jobdesk/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Jobdesk model, string tugasUtama)
        {
            var jobdesks = JobdeskHelper.LoadJobdesk();
            var existingJobdesk = jobdesks.FirstOrDefault(j => j.IdJobdesk == id);

            if (existingJobdesk == null)
                return NotFound();

            existingJobdesk.NamaJobdesk = model.NamaJobdesk;
            existingJobdesk.TugasUtama = ParseTugasUtama(tugasUtama);

            JobdeskHelper.SimpanJobdesk(jobdesks);

            return RedirectToAction(nameof(Index));
        }

        // Helper Method: Parsers
        private static List<string> ParseTugasUtama(string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? new List<string>()
                : input.Split(',').Select(t => t.Trim()).WhereNotNullOrEmpty().ToList();
        }
    }

    // Extension Method untuk kebersihan
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> WhereNotNullOrEmpty(this IEnumerable<string> source)
        {
            return source.Where(s => !string.IsNullOrWhiteSpace(s));
        }
    }
}
