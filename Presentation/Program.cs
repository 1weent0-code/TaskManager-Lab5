using TaskManager.Application.UseCases;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Presentation;

internal class Program
{
    private static void Main()
    {
        // Composition root: тут збираються конкретні залежності.
        var repository = new JsonTaskRepository("tasks.json");
        var createTask = new CreateTaskUseCase(repository);
        var getAllTasks = new GetAllTasksUseCase(repository);
        var completeTask = new CompleteTaskUseCase(repository);

        while (true)
        {
            Console.WriteLine("\n=== TASK MANAGER ===");
            Console.WriteLine("1. Додати завдання");
            Console.WriteLine("2. Показати всі завдання");
            Console.WriteLine("3. Завершити завдання");
            Console.WriteLine("0. Вийти");
            Console.Write("Ваш вибір: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Write("Введіть назву: ");
                    try
                    {
                        var task = createTask.Execute(Console.ReadLine() ?? string.Empty);
                        Console.WriteLine($"Завдання додано. ID: {task.Id}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "2":
                    var tasks = getAllTasks.Execute();
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Список завдань порожній.");
                        break;
                    }

                    foreach (var task in tasks)
                    {
                        var status = task.IsCompleted ? "Виконано" : "Не виконано";
                        Console.WriteLine($"{task.Id} | {task.Title} | {status}");
                    }
                    break;

                case "3":
                    Console.Write("Введіть ID завдання: ");
                    if (Guid.TryParse(Console.ReadLine(), out var id))
                        Console.WriteLine(completeTask.Execute(id)
                            ? "Завдання виконано!"
                            : "Завдання не знайдено.");
                    else
                        Console.WriteLine("Некоректний формат ID.");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }
    }
}
