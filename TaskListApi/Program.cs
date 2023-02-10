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

app.MapGet("/getTasks", async (ITaskListDbContext context) =>
{
    var query = context.Task.AsNoTracking();

    return Results.Ok(await query.ToListAsync());
});

app.MapGet("/getTaskById", async (int id, ITaskListDbContext context) =>
{
    var entity = await context.Task.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    if (entity is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(entity);
});

app.MapPost("/createTask", async (TaskEntity task, ITaskListDbContext context) =>
{
    var result = await context.Task.AddAsync(task);
    await context.SaveChangesAsync();
    return Results.Ok(result.Entity);
});

app.MapPut("/updateTask", async (TaskEntity task, ITaskListDbContext context) =>
{
    var res = context.Task.Update(task);
    await context.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/deleteTask", async (int id, ITaskListDbContext context) =>
{
    var entity = await context.Task.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    if (entity is null)
    {
        return Results.NotFound();
    }

    entity.IsDeleted = true;

    context.Task.Update(entity);
    await context.SaveChangesAsync();

    return Results.Ok(id);
});

app.Run();
