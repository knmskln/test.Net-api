using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc;
using test.Net_api.Service;

namespace test.Net_api.Controllers;

public class SpeedRecordControllerBenchmark
{
    private readonly SpeedRecordController _controller;
    private readonly SpeedRecord _testSpeedRecords;
    private readonly double _testThreshold;

    public SpeedRecordControllerBenchmark()
    {
        _controller = new SpeedRecordController(new SpeedRecordService());

        _testSpeedRecords  = new SpeedRecord
        {
            Timestamp = new DateTime(2023, 8, 13) + TimeSpan.FromMinutes(200),
            VehicleNumber = "4444 TT-2",
            Speed = 210
        };
        
        _testThreshold = 70.0;
    }

    [Benchmark]
    public void PostSpeedRecordBenchmark()
    {
        _controller.PostSpeedRecord(_testSpeedRecords).GetAwaiter().GetResult();
    }

    [Benchmark]
    public IActionResult GetSpeedRecordsForDateBenchmark()
    {
        return _controller.GetSpeedRecordsForDate("2023-08-13");
    }

    [Benchmark]
    public IActionResult GetSpeedRecordsExceededForDateBenchmark()
    {
        return _controller.GetSpeedRecordsExceededForDate("2023-08-13", _testThreshold);
    }

    [Benchmark]
    public IActionResult GetAllRecordsBenchmark()
    {
        return _controller.GetAllRecords("2023-08-13");
    }
    
    private List<SpeedRecord> GenerateRandomSpeedRecords(int count)
    {
        var records = new List<SpeedRecord>();
        var random = new Random();
        var fixedDate = new DateTime(2023, 8, 13);

        for (int i = 0; i < count; i++)
        {        
            var randomTimeSpan = TimeSpan.FromMinutes(random.Next(0, 60 * 24)); 

            var speedRecord = new SpeedRecord
            {
                Timestamp = fixedDate + randomTimeSpan,
                VehicleNumber = "2222 TT-2",
                Speed = 30 + random.NextDouble() * 90
            };
            records.Add(speedRecord);
        }

        return records;
    }

}