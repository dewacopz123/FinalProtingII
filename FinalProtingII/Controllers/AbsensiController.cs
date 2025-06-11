using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FinalProtingII.Helpers;
using FinalProtingII.Models;

public class AbsensiController : Controller
{
    // Load absensi dari file setiap akses (atau bisa dioptimalkan pakai cache jika perlu)
    private List<Absensi> LoadAbsensi()
    {
        // AbsensiHelper perlu diupdate untuk support Absensi (bukan Karyawan)
        return AbsensiHelper.LoadAbsensi();
    }

    // Simpan absensi ke file
    private void SaveAbsensi(List<Absensi> list)
    {
        AbsensiHelper.SaveAbsensi(list);
    }

    private bool SudahAbsensiHariIni(string namaKaryawan, DateTime tanggal, string status)
    {
        var absensiList = LoadAbsensi();

        return absensiList.Any(a =>
            a.NamaKaryawan == namaKaryawan &&
            a.Tanggal.Date == tanggal.Date &&
            a.Status == status);
    }

    private void TambahAbsensi(string namaKaryawan, string status)
    {
        var absensiList = LoadAbsensi();

        absensiList.Add(new Absensi
        {
            Id = absensiList.Any() ? absensiList.Max(a => a.Id) + 1 : 1,
            NamaKaryawan = namaKaryawan,
            Tanggal = DateTime.Now,
            Status = status
        });

        SaveAbsensi(absensiList);
    }

    public IActionResult Index()
    {
        var nama = HttpContext.Session.GetString("Username") ?? "Unknown";

        var absensiList = LoadAbsensi();

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
