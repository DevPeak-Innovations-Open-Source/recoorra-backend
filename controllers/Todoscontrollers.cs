// Endpoints/TodoEndpoints.cs
using ToDoApi.Models;

namespace ToDoApi.controllers;

public static class Todoscontrollers
{
    private static readonly List<Todo> todos = new()
    {
        new Todo { Id = 1, Title = "Learn .NET 9", IsCompleted = false },
        new Todo { Id = 2, Title = "Build a REST API", IsCompleted = true }
    };

    public static void MapTodoControllers(this WebApplication app)
    {
        app.MapGet("/api/todos", () => todos);

        app.MapGet("/api/todos/{id:int}", (int id) =>
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        });

        app.MapPost("/api/todos", (Todo todo) =>
        {
            todo.Id = todos.Max(t => t.Id) + 1;
            todos.Add(todo);
            return Results.Created($"/api/todos/{todo.Id}", todo);
        });

        app.MapPut("/api/todos/{id:int}", (int id, Todo updated) =>
        {
            var existing = todos.FirstOrDefault(t => t.Id == id);
            if (existing is null) return Results.NotFound();

            existing.Title = updated.Title;
            existing.IsCompleted = updated.IsCompleted;

            return Results.Ok(existing);
        });

        app.MapDelete("/api/todos/{id:int}", (int id) =>
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            if (todo is null) return Results.NotFound();

            todos.Remove(todo);
            return Results.NoContent();
        });
    }
}
