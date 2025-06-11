using FinalProtingII.Models;
using System.Text.Json;

public static class KaryawanHelper
{
    private static readonly string FilePath = "Data/karyawan.json"; // ganti ke lokasi yang simple

    public static List<Karyawan> LoadKaryawan()
    {
        if (!File.Exists(FilePath))
            return new List<Karyawan>();

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Karyawan>>(json) ?? new List<Karyawan>();
    }

    public static void SimpanKaryawan(List<Karyawan> list)
    {
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static Karyawan? GetById(int id)
    {
        return LoadKaryawan().FirstOrDefault(k => k.Id == id);
    }

    public static void TambahKaryawan(Karyawan karyawan)
    {
        var data = LoadKaryawan();
        data.Add(karyawan);
        SimpanKaryawan(data);
    }

    public static bool HapusKaryawan(int id)
    {
        var data = LoadKaryawan();
        var item = data.FirstOrDefault(k => k.Id == id);
        if (item != null)
        {
            data.Remove(item);
            SimpanKaryawan(data);
            return true;
        }
        return false;
    }
}
