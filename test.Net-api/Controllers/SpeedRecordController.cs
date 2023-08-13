using Microsoft.AspNetCore.Mvc;
using test.Net_api.Service;

namespace test.Net_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpeedRecordController : ControllerBase
{
    private readonly SpeedRecordService _speedRecordService;

    public SpeedRecordController(SpeedRecordService speedRecordService)
    {
        this._speedRecordService = speedRecordService;
    }

    [HttpPost]
    public async Task<IActionResult> PostSpeedRecord([FromBody] SpeedRecord speedRecord)
    {
        try
        {
            await _speedRecordService.SaveSpeedRecordAsync(speedRecord);
            return Ok("Speed record saved successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    [HttpGet("minMax")]
    [TimeRangeFilter]
    public IActionResult GetSpeedRecordsForDate(string date)
    {
        if (DateTime.TryParse(date, out DateTime requestedDate))
        {
            var records = _speedRecordService.GetMinMaxForDate(requestedDate);
            var speedRecords = records as SpeedRecord[] ?? records.ToArray();
            if (!speedRecords.Any())
            {
                return NotFound($"No speed records found for date {requestedDate:yyyy-MM-dd}.");
            }

            return Ok(new
            {
                SpeedRecords = speedRecords
            });
        }

        return BadRequest("Invalid date format. Please use the format yyyy-MM-dd.");
    }
    
    [HttpGet("speed-exceeded")]
    [TimeRangeFilter]
    public IActionResult GetSpeedRecordsExceededForDate(string date, double threshold)
    {
        if (!DateTime.TryParse(date, out DateTime parsedDate))
        {
            return BadRequest("Invalid date format. Please use format dd-mm-yyyy.");
        }

        var records = _speedRecordService.GetSpeedRecordsForDate(parsedDate, threshold);

        return Ok(records);
    }
    
    [HttpGet("getAll")]
    [TimeRangeFilter]
    public IActionResult GetAllRecords(string date)
    {
        if (DateTime.TryParse(date, out DateTime requestedDate))
        {
            var records = _speedRecordService.GetAll(requestedDate);
            var speedRecords = records as SpeedRecord[] ?? records.ToArray();
            if (!speedRecords.Any())
            {
                return NotFound($"No speed records found for date {requestedDate:yyyy-MM-dd}.");
            }

            return Ok(new
            {
                SpeedRecords = speedRecords
            });
        }

        return BadRequest("Invalid date format. Please use the format yyyy-MM-dd.");
    }
}