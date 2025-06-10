using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FinalProtingII.Models;

namespace FinalProtingII.Helpers
{
    public static class PenggajihanHelper
    {
        private static readonly string filePath = "data_karyawan.json";

        public static List<Karyawan> LoadData()
        {
            if (!File.Exists(filePath))
                return new List<Karyawan>();

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Karyawan>>(json) ?? new List<Karyawan>();
        }

        public static void SimpanData(List<Karyawan> daftarKaryawan)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(daftarKaryawan, options);
            File.WriteAllText(filePath, json);
        }
    }
}
