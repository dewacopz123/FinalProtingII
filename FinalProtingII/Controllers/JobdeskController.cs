using AplikasiAbsensi.Core.Helper;
using AplikasiAbsensi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AplikasiAbsensi.WebApi.Controllers
{
    [ApiController]
    [Route("jobdesk")]
    public class JobdeskController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetJobdesk()
        {
            var jobdeskList = JobdeskHelper.LoadJobdesk();
            return Ok(jobdeskList);
        }

        [HttpGet("{id}")]
        public IActionResult GetJobdeskById(int id)
        {
            var jobdesk = JobdeskHelper.GetById(id);
            if (jobdesk == null)
                return NotFound($"Jobdesk dengan ID {id} tidak ditemukan.");

            return Ok(jobdesk);
        }

        [HttpPost]
        public IActionResult PostJobdesk([FromBody] JobDesk newJobdesk)
        {
            if (newJobdesk == null || string.IsNullOrWhiteSpace(newJobdesk.NamaJobdesk))
                return BadRequest("Data jobdesk tidak valid.");
            if (newJobdesk.TugasUtama == null || newJobdesk.TugasUtama.Count == 0)
                return BadRequest("Tugas utama tidak boleh kosong.");


            var daftarJobdesk = JobdeskHelper.LoadJobdesk();
            newJobdesk.IdJobdesk = daftarJobdesk.Any()
                ? daftarJobdesk.Max(j => j.IdJobdesk) + 1
                : 1;

            JobdeskHelper.TambahJobdesk(newJobdesk);
            return Ok(newJobdesk);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteJobdesk(int id)
        {
            bool hasil = JobdeskHelper.HapusJobdesk(id);
            if (!hasil)
                return NotFound($"Jobdesk dengan ID {id} tidak ditemukan.");

            return Ok($"Jobdesk dengan ID {id} berhasil dihapus.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateJobdesk(int id, [FromBody] JobDesk updatedJobdesk)
        {
            var jobdeskList = JobdeskHelper.LoadJobdesk();
            var existing = jobdeskList.FirstOrDefault(j => j.IdJobdesk == id);
            if (existing == null) return NotFound("Jobdesk tidak ditemukan.");

            existing.NamaJobdesk = updatedJobdesk.NamaJobdesk;
            existing.TugasUtama = updatedJobdesk.TugasUtama ?? new();

            JobdeskHelper.SimpanJobdesk(jobdeskList);
            return Ok(existing);
        }
    }
}
