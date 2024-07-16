using WebApiBackgroundServiceExample.Solution1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHostedService<BackgroundTaskServiceParallelSolution1>()
    //.AddHostedService<BackgroundTaskServiceSequentialSolution1>()
    .AddSingleton<ITaskQueueSolution1, TaskQueueSolution1>()

    //.AddHostedService<BackgroundTaskServiceParallelSolution2>()
    ////.AddHostedService<BackgroundTaskServiceSequentialSolution2>()
    //.AddSingleton<ITaskQueueSolution2, TaskQueueSolution2>()
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
