using Microsoft.AspNetCore.Mvc;
using WebApiBackgroundServiceExample.Solution1;

namespace WebApiBackgroundServiceExample.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ITaskQueueSolution1 _taskQueue;
    private readonly ILogger<TestController> _logger;

    public TestController(
        ITaskQueueSolution1 taskQueue,
        ILogger<TestController> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    [HttpGet(Name = "Test")]
    public IActionResult Get()
    {
        var guid = Guid.NewGuid();

        _taskQueue.Enqueue(ct => RunTask($"{guid} - 1", 3000));
        _taskQueue.Enqueue(ct => RunTask($"{guid} - 2", 3000));
        _taskQueue.Enqueue(ct => RunTask($"{guid} - 3", 3000));

        return Ok();
    }

    private async Task RunTask(string text, int delay)
    {
        await Task.Delay(delay);
        Console.WriteLine(text);
    }
}
