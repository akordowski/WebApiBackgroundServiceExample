namespace WebApiBackgroundServiceExample.Solution1;

public interface ITaskQueueSolution1
{
    void Clear();
    void Enqueue(Func<CancellationToken, Task> func);
    bool TryDequeue(out Func<CancellationToken, Task> func);
}