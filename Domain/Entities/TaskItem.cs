namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }

    public TaskItem(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва завдання не може бути порожньою.");

        Id = Guid.NewGuid();
        Title = title.Trim();
        IsCompleted = false;
    }

    // Потрібен для десеріалізації JSON.
    public TaskItem()
    {
        Id = Guid.NewGuid();
        Title = string.Empty;
        IsCompleted = false;
    }

    public void Complete() => IsCompleted = true;
}
