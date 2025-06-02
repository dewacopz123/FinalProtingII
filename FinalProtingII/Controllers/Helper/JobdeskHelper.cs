using System.Text.Json;
using AplikasiAbsensi.Core.Models;

namespace AplikasiAbsensi.Core.Helper
{
    public static class JobdeskHelper
    {
        private static readonly string fileJobdeskPath = "data_jobdesk.json";

        public static List<JobDesk> LoadJobdesk()
        {
            if (!File.Exists(fileJobdeskPath)) return new List<JobDesk>();

            var json = File.ReadAllText(fileJobdeskPath);
            return JsonSerializer.Deserialize<List<JobDesk>>(json) ?? new List<JobDesk>();
        }

        public static void SimpanJobdesk(List<JobDesk> daftarJobdesk)
        {
            var json = JsonSerializer.Serialize(daftarJobdesk, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileJobdeskPath, json);
        }

        public static void TambahJobdesk(JobDesk jobdesk)
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

        public static JobDesk? GetById(int idJobdesk)
        {
            var daftarJobdesk = LoadJobdesk();
            return daftarJobdesk.FirstOrDefault(j => j.IdJobdesk == idJobdesk);
        }
    }
}
