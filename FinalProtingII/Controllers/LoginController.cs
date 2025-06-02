using Microsoft.AspNetCore.Mvc;
using AplikasiAbsensi.Core.Models;
using AplikasiAbsensi.Core.Services;
using System.Collections.Generic;

namespace AplikasiAbsensi.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        // Constructor dengan Dependency Injection LoginService
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        // GET: api/login
        [HttpGet]
        public IActionResult GetAllKaryawan()
        {
            List<Karyawan> allKaryawan = _loginService.GetAllKaryawan();
            return Ok(allKaryawan);
        }

        // POST: api/login
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nama) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { message = "Nama dan Email wajib diisi." });
            }

            var karyawan = _loginService.Login(request.Nama, request.Email);

            if (karyawan == null)
            {
                return Unauthorized(new { message = "Nama atau email tidak cocok." });
            }

            return Ok(new { message = "Login berhasil!", data = karyawan });
        }
    }


    public class LoginRequest
    {
        public string Nama { get; set; }
        public string Email { get; set; }
    }
}
