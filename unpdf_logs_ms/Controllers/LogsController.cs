using unpdf_logs_ms.Models;
using unpdf_logs_ms.Services;
using Microsoft.AspNetCore.Mvc;

namespace unpdf_logs_ms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly LogsService _logsService;

    public LogsController(LogsService logsService) =>
        _logsService = logsService;

    [HttpGet]
    public  async Task<List<Log>> Get() =>
        await _logsService.GetAsync();

    [HttpGet("(id)")]
    public async Task<List<Log>> Get(string id)
    {
        var book = await _logsService.GetAsync(id);

        /*
        if (book is null)
        {
            return NotFound();
        }
        */

        return book;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Log newLog)
    {
        DateTime now = DateTime.Now;
        
        newLog.Date = now.Add(new TimeSpan(-5, 0, 0));

        await _logsService.CreateAsync(newLog);

        return CreatedAtAction(nameof(Get), new { id = newLog.Id }, newLog);
    }

}
