using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace test.Net_api;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TimeRangeFilterAttribute : ActionFilterAttribute
{
    public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(1);
    public TimeSpan EndTime { get; set; } = TimeSpan.FromHours(18);

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        DateTime now = DateTime.Now;
        TimeSpan currentTime = now.TimeOfDay;

        if (currentTime < StartTime || currentTime > EndTime)
        {
            context.Result = new ContentResult
            {
                Content = "Outside allowed time range",
                StatusCode = 403
            };
        }
    }
}