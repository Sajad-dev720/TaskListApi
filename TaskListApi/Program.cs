using Microsoft.EntityFrameworkCore;
using TaskListApi.Context;
using TaskListApi.Model;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TaskDbContext");

builder.Services.AddDbContextPool<ITaskListDbContext, TaskListDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.MapPost("/createTask", async (TaskEntity entity, ITaskListDbContext context) =>
{
    var result = await context.Task.AddAsync(entity);
    await context.SaveChangesAsync();
    return Results.Ok(result.Entity);
});

app.Run();
