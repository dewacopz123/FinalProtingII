using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using FinalProtingII.Models; // Ganti dengan namespace model yang sesuai

public class AbsensiController : Controller
{
    // Simulasi database sementara (nanti bisa diganti dengan DB sesungguhnya)
    private static List<Absensi> absensiList = new List<Absensi>();

    public IActionResult Index()
    {
        return View(absensiList.OrderByDescending(a => a.Tanggal).ToList());
    }

    [HttpPost]
    public IActionResult MasukKerja()
    {
        var nama = HttpContext.Session.GetString("Username") ?? "Unknown";

        absensiList.Add(new Absensi
        {
            Id = absensiList.Count + 1,
            NamaKaryawan = nama,
            Tanggal = DateTime.Now,
            Status = "Masuk"
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SelesaiKerja()
    {
        var nama = HttpContext.Session.GetString("Username") ?? "Unknown";

        absensiList.Add(new Absensi
        {
            Id = absensiList.Count + 1,
            NamaKaryawan = nama,
            Tanggal = DateTime.Now,
            Status = "Selesai"
        });

        return RedirectToAction("Index");
    }
}
