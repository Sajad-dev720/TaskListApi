using Microsoft.EntityFrameworkCore;
using TaskListApi.Context;
using TaskListApi.Domain;
using AutoMapper;
using TaskListApi.Model;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TaskDbContext");

builder.Services.AddDbContextPool<ITaskListDbContext, TaskListDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

app.UseCors(options =>
{
    options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/getTasks", async (ITaskListDbContext context) =>
{
    var query = context.Task
        .AsNoTracking()
        .Where(w => !w.IsDeleted)
        .OrderByDescending(o => o.CreatedAt)
        .Select(x => new TaskModel
        {
            Id = x.Id,
            Title = x.Title,
            DeadLineDays = x.DeadLineDays,
            Description = x.Description,
            IsDone = x.IsDone,
            IsDeleted = x.IsDeleted,
            CreatedAt = x.CreatedAt,
            LastModifiedAt = x.LastModifiedAt,
        });

    return Results.Ok(await query.ToListAsync());
});

app.MapGet("/getTaskById", async (int id, ITaskListDbContext context, IMapper mapper) =>
{
    var entity = await context.Task.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    if (entity is null)
    {
        return Results.NotFound();
    }

    var model = mapper.Map<TaskModel>(entity);

    return Results.Ok(model);
});

app.MapPost("/createTask", async ([FromBody]TaskEntity task, ITaskListDbContext context, IMapper mapper) =>
{
    var res = await context.Task.AddAsync(task);
    await context.SaveChangesAsync();

    var model = mapper.Map<TaskModel>(res.Entity);

    return Results.Ok(model);
});

app.MapPut("/updateTask", async ([FromBody]TaskEntity task, ITaskListDbContext context, IMapper mapper) =>
{
    var res = context.Task.Update(task);
    await context.SaveChangesAsync();

    var model = mapper.Map<TaskModel>(res.Entity);

    return Results.Ok(model);
});

app.MapPut("/deleteTask", async ([FromBody]TaskModel model, ITaskListDbContext context) =>
{
    var entity = await context.Task.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);

    if (entity is null)
    {
        return Results.NotFound();
    }

    entity.IsDeleted = true;

    context.Task.Update(entity);
    await context.SaveChangesAsync();

    return Results.Ok(model.Id);
});

app.Run();
