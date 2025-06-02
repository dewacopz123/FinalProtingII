using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AplikasiAbsensi.Core.Models;

namespace AplikasiAbsensi.Core.Helpers
{
    public static class PresensiHelper
    {
        private const string FilePath = "data_presensi.json";

        // Simpan list presensi ke file JSON
        public static void SimpanPresensi(List<Karyawan> daftarPresensi)
        {
            try
            {
                var json = JsonSerializer.Serialize(daftarPresensi, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal menyimpan data presensi: {ex.Message}");
            }
        }

        // Muat data presensi dari file JSON
        public static List<Karyawan> LoadPresensi()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new List<Karyawan>();

                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Karyawan>>(json) ?? new List<Karyawan>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal membaca data presensi: {ex.Message}");
                return new List<Karyawan>();
            }
        }
    }
}
