using Microsoft.AspNetCore.Mvc;
using test.Net_api.Service;

namespace test.Net_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeedRecordController : ControllerBase
{
    private readonly SpeedMonitoringSystem speedMonitoringSystem;

    public SpeedRecordController(SpeedMonitoringSystem speedMonitoringSystem)
    {
        this.speedMonitoringSystem = speedMonitoringSystem;
    }

    [HttpPost]
    public async Task<IActionResult> PostSpeedRecord([FromBody] SpeedRecord speedRecord)
    {
        try
        {
            await speedMonitoringSystem.SaveSpeedRecordAsync(speedRecord);
            return Ok("Speed record saved successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    [HttpGet]
    public IActionResult GetSpeedRecordsForDate(string date)
    {
        if (DateTime.TryParse(date, out DateTime requestedDate))
        {
            var records = speedMonitoringSystem.GetSpeedRecordsForDate(requestedDate);
            var speedRecords = records as SpeedRecord[] ?? records.ToArray();
            if (!speedRecords.Any())
            {
                return NotFound($"No speed records found for date {requestedDate:yyyy-MM-dd}.");
            }

            var maxSpeedRecord = speedRecords.MaxBy(r => r.Speed);
            var minSpeedRecord = speedRecords.MinBy(r => r.Speed);

            return Ok(new
            {
                MaxSpeedRecord = maxSpeedRecord,
                MinSpeedRecord = minSpeedRecord
            });
        }

        return BadRequest("Invalid date format. Please use the format yyyy-MM-dd.");
    }
    
    [HttpGet("speed-exceeded")]
    public IActionResult GetSpeedRecordsExceededForDate(string date, double threshold)
    {
        if (!DateTime.TryParse(date, out DateTime parsedDate))
        {
            return BadRequest("Invalid date format. Please use format dd-mm-yyyy.");
        }

        var records = speedMonitoringSystem.GetSpeedRecordsForDate(parsedDate)
            .Where(record => record.Speed > threshold)
            .ToList();

        if (records.Count == 0)
        {
            return NotFound("No speed records found for the specified date and threshold.");
        }

        return Ok(records);
    }
}