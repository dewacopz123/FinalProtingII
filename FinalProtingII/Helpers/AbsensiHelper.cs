using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FinalProtingII.Models;

namespace FinalProtingII.Helpers
{
    public static class AbsensiHelper
    {
        private const string FilePath = "Data/absensi.json";

        // Simpan list absensi ke file JSON
        public static void SaveAbsensi(List<Absensi> daftarAbsensi)
        {
            try
            {
                var json = JsonSerializer.Serialize(daftarAbsensi, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal menyimpan data absensi: {ex.Message}");
            }
        }

        // Muat data absensi dari file JSON
        public static List<Absensi> LoadAbsensi()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new List<Absensi>();

                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<List<Absensi>>(json) ?? new List<Absensi>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal membaca data absensi: {ex.Message}");
                return new List<Absensi>();
            }
        }
    }
}
