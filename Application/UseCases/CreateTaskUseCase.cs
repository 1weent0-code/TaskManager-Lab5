using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.UseCases;

public class CreateTaskUseCase
{
    private readonly ITaskRepository _repository;

    public CreateTaskUseCase(ITaskRepository repository) => _repository = repository;

    public TaskItem Execute(string title)
    {
        var task = new TaskItem(title);
        _repository.Add(task);
        return task;
    }
}
