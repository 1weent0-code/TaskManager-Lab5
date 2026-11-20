using System.Text.Json;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonTaskRepository(string filePath) => _filePath = filePath;

    private List<TaskItem> Load()
    {
        if (!File.Exists(_filePath)) return new List<TaskItem>();

        var json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json)) return new List<TaskItem>();

        return JsonSerializer.Deserialize<List<TaskItem>>(json, _options) ?? new List<TaskItem>();
    }

    private void Save(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, _options);
        File.WriteAllText(_filePath, json);
    }

    public void Add(TaskItem task)
    {
        var tasks = Load();
        tasks.Add(task);
        Save(tasks);
    }

    public List<TaskItem> GetAll() => Load();

    public TaskItem? GetById(Guid id) => Load().FirstOrDefault(task => task.Id == id);

    public void Update(TaskItem task)
    {
        var tasks = Load();
        var index = tasks.FindIndex(item => item.Id == task.Id);
        if (index < 0) return;

        tasks[index] = task;
        Save(tasks);
    }
}
