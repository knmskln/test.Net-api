namespace test.Net_api.Service;

public class SpeedMonitoringSystem
{
    private const string DataDirectory = "Data";

    public async Task SaveSpeedRecordAsync(SpeedRecord record)
    {
        var filePath = GetFilePathForDate(record.Timestamp.Date);
        var data = $"{record.Timestamp},{record.VehicleNumber},{record.Speed}\n";

        var projectDirectory = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(projectDirectory, DataDirectory, filePath);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        await File.AppendAllTextAsync(fullPath, data);
    }

    private string GetFilePathForDate(DateTime date)
    {
        var fileName = $"{date:yyyy-MM-dd}.csv";
        return fileName;
    }
    
    public IEnumerable<SpeedRecord> GetSpeedRecordsForDate(DateTime date)
    {
        var filePath = GetFilePathForDate(date);

        var projectDirectory = Directory.GetCurrentDirectory();
        var fullPath = Path.Combine(projectDirectory, DataDirectory, filePath);

        if (!File.Exists(fullPath))
        {
            return Enumerable.Empty<SpeedRecord>();
        }

        var lines = File.ReadAllLines(fullPath);
        return lines.Select(line =>
        {
            var parts = line.Split(',');
            return new SpeedRecord
            {
                Timestamp = DateTime.Parse(parts[0]),
                VehicleNumber = parts[1],
                Speed = double.Parse(parts[2] + "," + parts[3])
            };
        });
    }
}