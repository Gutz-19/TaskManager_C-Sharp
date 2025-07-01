using System.Text.Json;
using System.Collections.Generic;
using System.IO;

namespace TaskManager;

public class TaskManager
{
    private List<Task> tasks = new List<Task>();
    private const string FilePath = "tasks.json";

    public void AddTask(Task task)
    {
        task.Id = tasks.Count + 1;
        tasks.Add(task);
        SaveTasks();
    }

    public List<Task> GetTasks() => tasks;

    public void UpdateTask(int id, Task updatedTask)
    {
        var task = tasks.Find(t => t.Id == id);
        if (task != null)
        {
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.Status = updatedTask.Status;
            SaveTasks();
        }
    }

    public void DeleteTask(int id)
    {
        tasks.RemoveAll(t => t.Id == id);
        SaveTasks();
    }

    public List<Task> FilterTasksByStatus(string status) =>
        tasks.FindAll(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

    public void LoadTasks()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            tasks = JsonSerializer.Deserialize<List<Task>>(json) ?? new List<Task>();
        }
    }

    private void SaveTasks()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(tasks, options);
        File.WriteAllText(FilePath, json);
    }
}