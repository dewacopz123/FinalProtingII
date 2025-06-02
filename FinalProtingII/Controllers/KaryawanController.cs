using AplikasiAbsensi.Core.Helper;
using AplikasiAbsensi.Core.Helpers;
using AplikasiAbsensi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Aplikasi_Absensi_Perusahaan.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KaryawanController : ControllerBase
    {

        [HttpGet("karyawan")]
        public IActionResult GetKaryawan()
        {
            var karyawanList = KaryawanHelper.LoadKaryawan();
            return Ok(karyawanList);
        }
        
        

    }
}
