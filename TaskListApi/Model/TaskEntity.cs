namespace TaskListApi.Model;

public class TaskEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Descrption { get; set; }

    public bool IsDone { get; set; }

    public int DeadLineDays { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastModifiedAt { get; set; }

    public DateTime ExpiresAt => CreatedAt.AddDays(DeadLineDays);

    public bool IsDeleted { get; set; } = false;
}
