using DNTPersianUtils.Core;

namespace TaskListApi.Model;

public class TaskModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsDone { get; set; }

    public int DeadLineDays { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastModifiedAt { get; set; }

    public DateTime ExpiresAt => CreatedAt.AddDays(DeadLineDays);

    public string ExpiresAtShamsi => ExpiresAt.ToShortPersianDateTimeString();

    public string CreatedAtShamsi => CreatedAt.ToShortPersianDateTimeString();

    public bool IsDeleted { get; set; } = false;
}
