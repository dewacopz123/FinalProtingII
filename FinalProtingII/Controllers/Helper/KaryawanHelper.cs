using System.Text.Json;
using AplikasiAbsensi.Core.Models;

namespace AplikasiAbsensi.Core.Helper
{
    public static class KaryawanHelper
    {
        private static readonly string filePath = "data_karyawan.json";
        private static readonly string fileJobdeskPath = "data_jobdesk.json";

        public static List<Karyawan> LoadKaryawan()
        {
            if (!File.Exists(filePath)) return new List<Karyawan>();

            var json = File.ReadAllText(filePath);
            var karyawanList = JsonSerializer.Deserialize<List<Karyawan>>(json) ?? new List<Karyawan>();

            // Load jobdesk list
            List<JobDesk> jobdeskList = JobdeskHelper.LoadJobdesk();

            // Pasangkan Jobdesk ke Karyawan berdasarkan JobdeskId
            foreach (var karyawan in karyawanList)
            {
                karyawan.Jobdesk = jobdeskList.FirstOrDefault(j => j.IdJobdesk == karyawan.JobdeskId);
            }

            return karyawanList;
        }

        public static void SimpanData<T>(List<T> daftarKaryawan)
        {
            var json = JsonSerializer.Serialize(daftarKaryawan, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static void TambahKaryawan(Karyawan karyawan)
        {
            var data = LoadKaryawan();
            data.Add(karyawan);
            SimpanData(data);
        }

        public static bool HapusKaryawan(int idKaryawan)
        {
            var data = LoadKaryawan();
            var karyawan = data.FirstOrDefault(k => k.Id_Karyawan == idKaryawan);
            if (karyawan != null)
            {
                data.Remove(karyawan);
                SimpanData(data);
                return true;
            }
            return false;
        }
    }
}
