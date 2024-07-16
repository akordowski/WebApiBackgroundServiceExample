namespace WebApiBackgroundServiceExample.Solution1;

public class BackgroundTaskServiceParallelSolution1 : BackgroundService
{
    private readonly ITaskQueueSolution1 _taskQueue;
    private readonly ILogger<BackgroundTaskServiceParallelSolution1> _logger;

    public BackgroundTaskServiceParallelSolution1(
        ITaskQueueSolution1 taskQueue,
        ILogger<BackgroundTaskServiceParallelSolution1> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }

    // As the ExecuteAsync can block the startup of the host this is the preferred solution 
    // https://blog.stephencleary.com/2020/05/backgroundservice-gotcha-startup.html
    // https://github.com/dotnet/runtime/issues/36063

    protected override Task ExecuteAsync(CancellationToken cancellationToken) => Task.Run(async () =>
    {
        var runningTasks = new List<Task>();

        while (!cancellationToken.IsCancellationRequested)
        {
            while (_taskQueue.TryDequeue(out var func))
            {
                var task = func(cancellationToken);
                runningTasks.Add(task);
            }

            try
            {
                if (runningTasks.Count > 0)
                {
                    var completedTask = await Task.WhenAny(runningTasks);
                    runningTasks.Remove(completedTask);
                }
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