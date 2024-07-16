namespace WebApiBackgroundServiceExample.Solution2;

public class BackgroundTaskServiceSequentialSolution2 : BackgroundService
{
    private readonly ITaskQueueSolution2 _taskQueue;
    private readonly ILogger<BackgroundTaskServiceSequentialSolution2> _logger;

    public BackgroundTaskServiceSequentialSolution2(
        ITaskQueueSolution2 taskQueue,
        ILogger<BackgroundTaskServiceSequentialSolution2> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var task = await _taskQueue.DequeueAsync(cancellationToken);

            if (task is null)
            {
                continue;
            }

            try
            {
                await task(cancellationToken);
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
    }
}