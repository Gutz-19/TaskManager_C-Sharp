using System;
using ConsoleTables;

namespace TaskManager;

class Program
{
    static readonly string[] validStatuses = { "To Do", "In Progress", "Done" };

    static void Main()
    {
        var manager = new TaskManager();
        manager.LoadTasks();

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== Task Manager ===");
            Console.ResetColor();
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View All Tasks");
            Console.WriteLine("3. Update Task");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Filter Tasks by Status");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Title: ");
                    var title = Console.ReadLine() ?? "";
                    Console.Write("Description: ");
                    var desc = Console.ReadLine() ?? "";
                    Console.Write("Due Date (yyyy-MM-dd): ");
                    DateTime dueDate;
                    bool isValidDate = DateTime.TryParse(Console.ReadLine(), out dueDate);
                    Console.Write("Status (To Do/In Progress/Done): ");
                    var status = Console.ReadLine() ?? "To Do";

                    // Input validation
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Title cannot be empty. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }
                    if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid status. Use: To Do, In Progress, or Done. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }
                    if (!isValidDate || dueDate < DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid or past due date. Use yyyy-MM-dd format. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }

                    manager.AddTask(new Task { Title = title, Description = desc, DueDate = dueDate, Status = status });
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Task added successfully! Press any key...");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;

                case "2":
                    DisplayTasks(manager.GetTasks());
                    Console.ReadKey();
                    break;

                case "3":
                    Console.Write("Enter Task ID to update: ");
                    if (!int.TryParse(Console.ReadLine(), out int id))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid ID. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }
                    Console.Write("New Title: ");
                    title = Console.ReadLine() ?? "";
                    Console.Write("New Description: ");
                    desc = Console.ReadLine() ?? "";
                    Console.Write("New Due Date (yyyy-MM-dd): ");
                    isValidDate = DateTime.TryParse(Console.ReadLine(), out dueDate);
                    Console.Write("New Status (To Do/In Progress/Done): ");
                    status = Console.ReadLine() ?? "To Do";

                    // Input validation
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Title cannot be empty. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }
                    if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid status. Use: To Do, In Progress, or Done. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }
                    if (!isValidDate || dueDate < DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid or past due date. Use yyyy-MM-dd format. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }

                    manager.UpdateTask(id, new Task { Title = title, Description = desc, DueDate = dueDate, Status = status });
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Task updated successfully! Press any key...");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;

                case "4":
                    Console.Write("Enter Task ID to delete: ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid ID. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }
                    manager.DeleteTask(id);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Task deleted successfully! Press any key...");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;

                case "5":
                    Console.Write("Enter Status to filter (To Do/In Progress/Done): ");
                    status = Console.ReadLine() ?? "";
                    if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(status))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error: Invalid status. Use: To Do, In Progress, or Done. Press any key...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    }
                    DisplayTasks(manager.FilterTasksByStatus(status));
                    Console.ReadKey();
                    break;

                case "6":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Exiting... Press any key...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Invalid option. Press any key...");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void DisplayTasks(List<Task> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("No tasks found.");
            Console.ResetColor();
            return;
        }

        var table = new ConsoleTable("ID", "Title", "Status", "Due Date", "Description");
        table.Configure(o => o.EnableCount = false);
        foreach (var task in tasks)
        {
            table.AddRow(task.Id, task.Title, task.Status, task.DueDate.ToString("yyyy-MM-dd"), task.Description);
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        table.Write();
        Console.ResetColor();
        Console.WriteLine("\nPress any key to continue...");
    }
}