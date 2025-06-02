using AplikasiAbsensi.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AplikasiAbsensi.WebApi.Controllers
{
    public class PresensiController : Controller
    {
        [HttpGet("presensi")]
        public IActionResult GetPresensi()
        {
            var presensiList = PresensiHelper.LoadPresensi();
            return Ok(presensiList);
        }
    }
}
