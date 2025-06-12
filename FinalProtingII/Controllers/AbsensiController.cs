using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FinalProtingII.Helpers;
using FinalProtingII.Models;

public class AbsensiController : Controller
{
    public IActionResult Index()
    {
        var nama = HttpContext.Session.GetString("Username") ?? "Unknown";
        var role = HttpContext.Session.GetString("role") ?? "Unknown";

        var absensiList = LoadAbsensi();

        List<Absensi> absensiUser;

        if (role == "admin")
        {
            // Admin melihat semua absensi
            absensiUser = absensiList.OrderByDescending(a => a.Tanggal).ToList();
        }
        else
        {
            // Karyawan hanya melihat absensi mereka sendiri
            absensiUser = absensiList
                .Where(a => a.NamaKaryawan == nama)
                .OrderByDescending(a => a.Tanggal)
                .ToList();
        }

        return View(absensiUser);
    }


    private List<Absensi> LoadAbsensi()
    {
        return AbsensiHelper.LoadAbsensi();
    }

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

        // Catat absensi dengan nama user yang login
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

        // Catat absensi dengan nama user yang login
        TambahAbsensi(nama, "Selesai");

        return RedirectToAction("Index");
    }



    [HttpGet]
    public IActionResult Search(string karyawanName)
    {
        var karyawanList = KaryawanJobdeskService.SearchKaryawan(nama: karyawanName);

        ViewBag.KaryawanNames = KaryawanHelper.LoadKaryawan()
            .Select(k => k.Nama)
            .Distinct()
            .ToList();

        return View("Index", karyawanList);
    }
}
