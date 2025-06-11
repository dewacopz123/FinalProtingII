using FinalProtingII.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FinalProtingII.Helpers
{
    public static class PenggajianHelper
    {
        private static readonly string filePath = "Data/penggajian.json";

        public static List<Penggajian> LoadPenggajian()
        {
            if (!File.Exists(filePath))
                return new List<Penggajian>();

            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Penggajian>>(json) ?? new List<Penggajian>();
        }

        public static void SavePenggajian(List<Penggajian> list)
        {
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void SavePenggajian(Penggajian item)
        {
            var list = LoadPenggajian();
            int nextId = list.Count > 0 ? list.Max(p => p.Id) + 1 : 1;
            item.Id = nextId;
            list.Add(item);
            SavePenggajian(list);
        }

        public static void UpdatePenggajian(Penggajian item)
        {
            var list = LoadPenggajian();
            var existing = list.FirstOrDefault(p => p.Id == item.Id);
            if (existing != null)
            {
                existing.IdKaryawan = item.IdKaryawan;
                existing.Tanggal = item.Tanggal;
                existing.GajiPokok = item.GajiPokok;
                SavePenggajian(list);
            }
        }

        public static void DeletePenggajian(int id)
        {
            var list = LoadPenggajian();
            var target = list.FirstOrDefault(p => p.Id == id);
            if (target != null)
            {
                list.Remove(target);
                SavePenggajian(list);
            }
        }
    }
}
