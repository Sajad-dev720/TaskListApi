using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskListApi.Configurations;
using TaskListApi.Model;

namespace TaskListApi.Context;

public class TaskListDbContext : DbContext, ITaskListDbContext
{
	public TaskListDbContext(DbContextOptions<TaskListDbContext> options) : base(options)
	{
	}
    public DbSet<TaskEntity> Task { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<TaskEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.Now;
                    entry.Entity.LastModifiedAt = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.LastModifiedAt = DateTime.Now;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(TaskConfiguration).Assembly);
        base.OnModelCreating(builder);
    }
}
