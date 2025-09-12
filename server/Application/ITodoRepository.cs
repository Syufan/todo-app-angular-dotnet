using Server.Domain;
namespace Server.Application;
public interface ITodoRepository
{   // Read Operation - Get all todos
    Task<IReadOnlyList<TodoItem>> ListAsync(CancellationToken ct);

    // Write Operation - Create new todo
    Task<TodoItem> AddAsync(string title, CancellationToken ct);

    // Write Operation - Delete by ID, returns success/failure
    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
