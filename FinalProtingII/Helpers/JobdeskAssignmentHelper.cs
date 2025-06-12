using FinalProtingII.Models;
using System.Text.Json;

public class JobdeskAssignmentHelper
{
    private static string FilePath = Path.Combine(AppContext.BaseDirectory, "Data", "jobdesk_assignment.json");

    public static List<JobdeskAssignment> LoadAssignments()
    {
        if (!File.Exists(FilePath))
            return new List<JobdeskAssignment>();

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<JobdeskAssignment>>(json) ?? new List<JobdeskAssignment>();
    }

    public static void SimpanAssignments(List<JobdeskAssignment> assignments)
    {
        var directory = Path.GetDirectoryName(FilePath);
        if (!Directory.Exists(directory!))
            Directory.CreateDirectory(directory!);

        var json = JsonSerializer.Serialize(assignments, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static void TambahAssignment(JobdeskAssignment assignment)
    {
        // Validasi assignment
        if (assignment.JobdeskId == 0 || assignment.KaryawanId == 0)
            return;

        var assignments = LoadAssignments();
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
