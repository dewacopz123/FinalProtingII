using FinalProtingII.Helpers;
using FinalProtingII.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public static class KaryawanJobdeskService
{
    // Cari karyawan berdasarkan Id dan/atau Nama
    public static List<Karyawan> SearchKaryawan(int? id = null, string nama = null)
    {
        var allKaryawans = KaryawanHelper.LoadKaryawan();
        var result = allKaryawans;

        if (id.HasValue)
            result = result.Where(k => k.Id == id.Value).ToList();

        if (!string.IsNullOrWhiteSpace(nama))
            result = result.Where(k => !string.IsNullOrEmpty(k.Nama) && k.Nama.Contains(nama, StringComparison.OrdinalIgnoreCase)).ToList();

        return result;
    }

    // Cari jobdesk berdasarkan Id dan/atau Nama
    public static List<Jobdesk> SearchJobdesk(int? id = null, string nama = null)
    {
        var allJobdesks = JobdeskHelper.LoadJobdesk();
        var result = allJobdesks;

        if (id.HasValue)
            result = result.Where(j => j.IdJobdesk == id.Value).ToList();

        if (!string.IsNullOrWhiteSpace(nama))
            result = result.Where(j => !string.IsNullOrEmpty(j.NamaJobdesk) && j.NamaJobdesk.Contains(nama, StringComparison.OrdinalIgnoreCase)).ToList();

        return result;
    }

    // Mapping jobdesk untuk list karyawan tertentu
    public static Dictionary<int, List<string>> GetJobdeskForKaryawans(List<int> karyawanIds)
    {
        var allJobdesks = JobdeskHelper.LoadJobdesk();
        var assignments = JobdeskAssignmentHelper.LoadAssignments();

        var map = new Dictionary<int, List<string>>();

        foreach (var kId in karyawanIds)
        {
            var assignedJobdeskIds = assignments
                .Where(a => a.KaryawanId == kId)
                .Select(a => a.JobdeskId)
                .ToList();

            var jobdeskNames = allJobdesks
                .Where(j => assignedJobdeskIds.Contains(j.IdJobdesk))
                .Select(j => j.NamaJobdesk)
                .ToList();

            map[kId] = jobdeskNames;
        }

        return map;
    }

    // Mapping karyawan untuk list jobdesk tertentu
    public static Dictionary<int, List<string>> GetKaryawansForJobdesks(List<int> jobdeskIds)
    {
        var allKaryawans = KaryawanHelper.LoadKaryawan();
        var assignments = JobdeskAssignmentHelper.LoadAssignments();

        var map = new Dictionary<int, List<string>>();

        foreach (var jId in jobdeskIds)
        {
            var assignedKaryawanIds = assignments
                .Where(a => a.JobdeskId == jId)
                .Select(a => a.KaryawanId)
                .ToList();

            var karyawanNames = allKaryawans
                .Where(k => assignedKaryawanIds.Contains(k.Id))
                .Select(k => k.Nama)
                .ToList();

            map[jId] = karyawanNames;
        }

        return map;
    }
}
