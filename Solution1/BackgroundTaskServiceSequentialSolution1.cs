namespace WebApiBackgroundServiceExample.Solution1;

public class BackgroundTaskServiceSequentialSolution1 : BackgroundService
{
    private readonly ITaskQueueSolution1 _taskQueue;
    private readonly ILogger<BackgroundTaskServiceSequentialSolution1> _logger;

    public BackgroundTaskServiceSequentialSolution1(
        ITaskQueueSolution1 taskQueue,
        ILogger<BackgroundTaskServiceSequentialSolution1> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    // As the ExecuteAsync can block the startup of the host this is the preferred solution 
    // https://blog.stephencleary.com/2020/05/backgroundservice-gotcha-startup.html
    // https://github.com/dotnet/runtime/issues/36063

    protected override Task ExecuteAsync(CancellationToken cancellationToken) => Task.Run(async () =>
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!_taskQueue.TryDequeue(out var func))
            {
                continue;
            }

            try
            {
                await func(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during background task execution.");
            }
        }

        if (cancellationToken.IsCancellationRequested)
        {
            _taskQueue.Clear();
        }
    }, cancellationToken);
}
