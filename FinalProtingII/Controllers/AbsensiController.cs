using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FinalProtingII.Models;

public class AbsensiController : Controller
{
    private static List<Absensi> absensiList = new List<Absensi>();

    // Method generic untuk cek absensi berdasar status
    private bool SudahAbsensiHariIni(string namaKaryawan, DateTime tanggal, string status)
    {
        return absensiList.Any(a =>
            a.NamaKaryawan == namaKaryawan &&
            a.Tanggal.Date == tanggal.Date &&
            a.Status == status);
    }

    // Method generic untuk tambah absensi
    private void TambahAbsensi(string namaKaryawan, string status)
    {
        absensiList.Add(new Absensi
        {
            Id = absensiList.Count + 1,
            NamaKaryawan = namaKaryawan,
            Tanggal = DateTime.Now,
            Status = status
        });
    }

    public IActionResult Index()
    {
        var nama = HttpContext.Session.GetString("Username") ?? "Unknown";

        // Filter absensi sesuai username session
        var absensiUser = absensiList
            .Where(a => a.NamaKaryawan == nama)
            .OrderByDescending(a => a.Tanggal)
            .ToList();

        return View(absensiUser);
    }


    [HttpPost]
    public IActionResult MasukKerja()
    {
        var nama = HttpContext.Session.GetString("Username") ?? "Unknown";
        var today = DateTime.Today;

        if (SudahAbsensiHariIni(nama, today, "Masuk"))
        {
            TempData["Error"] = "Anda sudah melakukan Masuk Kerja hari ini.";
            return RedirectToAction("Index");
        }

        TambahAbsensi(nama, "Masuk");
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SelesaiKerja()
    {
        var nama = HttpContext.Session.GetString("Username") ?? "Unknown";
        var today = DateTime.Today;

        if (!SudahAbsensiHariIni(nama, today, "Masuk"))
        {
            TempData["Error"] = "Anda belum melakukan Masuk Kerja hari ini.";
            return RedirectToAction("Index");
        }

        if (SudahAbsensiHariIni(nama, today, "Selesai"))
        {
            TempData["Error"] = "Anda sudah melakukan Selesai Kerja hari ini.";
            return RedirectToAction("Index");
        }

        TambahAbsensi(nama, "Selesai");
        return RedirectToAction("Index");
    }
}
