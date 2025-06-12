using FinalProtingII.Models;
using System.Text.Json;

namespace FinalProtingII.Helpers
{
    public class JobdeskAssignmentHelper
    {
        private static string FilePath = "Data/jobdesk_assignment.json";

        public static List<JobdeskAssignment> LoadAssignments()
        {
            if (!File.Exists(FilePath))
                return new List<JobdeskAssignment>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<JobdeskAssignment>>(json) ?? new List<JobdeskAssignment>();
        }

        public static void SimpanAssignments(List<JobdeskAssignment> assignments)
        {
            // ✅ Buat folder "Data" jika belum ada
            var directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            var json = JsonSerializer.Serialize(assignments, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static void TambahAssignment(JobdeskAssignment assignment)
        {
            var assignments = LoadAssignments();
            // Cegah duplikasi
            if (!assignments.Any(a => a.JobdeskId == assignment.JobdeskId && a.KaryawanId == assignment.KaryawanId))
            {
                assignments.Add(assignment);
                SimpanAssignments(assignments);
            }
        }

        public static void HapusAssignmentsByJobdesk(int jobdeskId)
        {
            var assignments = LoadAssignments();
            assignments = assignments.Where(a => a.JobdeskId != jobdeskId).ToList();
            SimpanAssignments(assignments);
        }

        public static void HapusAssignment(int jobdeskId, int karyawanId)
        {
            var assignments = LoadAssignments();
            assignments = assignments.Where(a => !(a.JobdeskId == jobdeskId && a.KaryawanId == karyawanId)).ToList();
            SimpanAssignments(assignments);
        }
    }
}
