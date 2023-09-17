using test.Net_api.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SpeedRecordService>();
builder.Services.AddControllers();

// BenchmarkRunner.Run<SpeedRecordControllerBenchmark>();
// BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();