using System.Text.Json;
using FinalProtingII.Models;

namespace FinalProtingII.Helpers
{
    public static class JobdeskHelper
    {
        private static readonly string fileJobdeskPath = "data_jobdesk.json";

        public static List<Jobdesk> LoadJobdesk()
        {
            if (!File.Exists(fileJobdeskPath)) return new List<Jobdesk>();

            var json = File.ReadAllText(fileJobdeskPath);
            return JsonSerializer.Deserialize<List<Jobdesk>>(json) ?? new List<Jobdesk>();
        }

        public static void SimpanJobdesk(List<Jobdesk> daftarJobdesk)
        {
            var json = JsonSerializer.Serialize(daftarJobdesk, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileJobdeskPath, json);
        }

        public static void TambahJobdesk(Jobdesk jobdesk)
        {
            var data = LoadJobdesk();
            data.Add(jobdesk);
            SimpanJobdesk(data);
        }

        public static bool HapusJobdesk(int idJobdesk)
        {
            var data = LoadJobdesk();
            var jobdesk = data.FirstOrDefault(j => j.IdJobdesk == idJobdesk);
            if (jobdesk != null)
            {
                data.Remove(jobdesk);
                SimpanJobdesk(data);
                return true;
            }
            return false;
        }

        public static Jobdesk? GetById(int idJobdesk)
        {
            var daftarJobdesk = LoadJobdesk();
            return daftarJobdesk.FirstOrDefault(j => j.IdJobdesk == idJobdesk);
        }
    }
}
