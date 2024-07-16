namespace WebApiBackgroundServiceExample.Solution2;

public class BackgroundTaskServiceParallelSolution2 : BackgroundService
{
    private readonly ITaskQueueSolution2 _taskQueue;
    private readonly ILogger<BackgroundTaskServiceParallelSolution2> _logger;

    public BackgroundTaskServiceParallelSolution2(
        ITaskQueueSolution2 taskQueue,
        ILogger<BackgroundTaskServiceParallelSolution2> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var runningTasks = new List<Task>();

        while (!cancellationToken.IsCancellationRequested)
        {
            while (true)
            {
                var func = await _taskQueue.DequeueAsync(cancellationToken);

                if (func is null)
                {
                    break;
                }

                var task = func(cancellationToken);
                runningTasks.Add(task);
            }

            if (runningTasks.Count == 0)
            {
                continue;
            }

            try
            {
                var completedTask = await Task.WhenAny(runningTasks);
                runningTasks.Remove(completedTask);
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